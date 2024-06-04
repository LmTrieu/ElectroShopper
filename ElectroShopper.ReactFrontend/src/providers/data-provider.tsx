import type { DataProvider } from "@refinedev/core";

const API_URL = "https://localhost:7265/api";

export const dataProvider: DataProvider = {
    getOne: async ({ resource, id }) => {
        const response = await fetch(`${API_URL}/${resource}/detail/${id}`);

        if (response.status < 200 || response.status > 299) throw response;

        const data = await response.json();
        const extractedData = data.data;

        return { data: extractedData };
    },
    update: async ({ resource, id, variables, meta }) => {        
        const params = new URLSearchParams();
        const { headers, hasBody, hasPicture } = meta;

        let response;
        if(hasPicture){
          const fd = new FormData();

          // Assuming 'variables' contains the data for other form fields
          for (const key in variables) {
            fd.append(key, variables[key]);
          }

          // Handle file upload (replace 'imageFile' with the actual field name)
          if (variables.productImage) {
            const file = variables.productImage.file;
            fd.append('ProductImage', file, file.name); // Append file data with filename
          }

          response = await fetch(`${API_URL}/${resource}/patch/${id}?${params}`, {
            method: "PATCH",
            body: fd,
            headers: {
              ...headers,
              Authorization: `Bearer ${localStorage.getItem("access_token")}`,
            },
          });
        }
        if(!hasBody){
          Object.entries(variables).forEach(
            ([key, value]) => 
              params.append(key, value)
          );
          response = await fetch(`${API_URL}/${resource}/patch/${id}?${params}`, {
            method: "PATCH",
            headers: {
              ...headers,
              Authorization: `Bearer ${localStorage.getItem("access_token")}`,
            },
          });
        }
        else{
          response = await fetch(`${API_URL}/${resource}/patch/${id}?${params}`, {
            method: "PATCH",
            body: JSON.stringify(variables),
            headers: {
              ...headers,
              Authorization: `Bearer ${localStorage.getItem("access_token")}`,
            },
          });
        }        
    
        if (response.status < 200 || response.status > 299) throw response;
    
        const data = await response.json();
        const extractedData = data.data;
    
        return { data: extractedData };
    },
    getList: async ({ resource, pagination}) => {
      const params = new URLSearchParams();

      if (pagination) {
        params.append("PageNumber", (pagination.current).toString());
        params.append("PageSize", (pagination.pageSize).toString());
      }      

      const response = await fetch(`${API_URL}/${resource}?${params}`);

      if (response.status < 200 || response.status > 299) throw response;
  
      // Extract the x-pagination header
      const paginationHeader = response.headers.get('x-pagination');
      const mDataPagination = JSON.parse(paginationHeader);

      const data = await response.json();
      const extractedData = data.data;
      
      return {
        data: extractedData,
        total: mDataPagination.TotalCount ?? 10, 
      };
    },
    create: async ({ resource, variables, meta }) => {
      const { headers, hasBody, hasPicture } = meta;
      
      const params = new URLSearchParams();
      let response;
      if (hasPicture) {
        const fd = new FormData();
    
        Object.entries(variables).forEach(
          ([key, value]) => 
            params.append(key, value)
        );
        params.delete("productImage");

        if (variables.productImage?.length) {
          const file = variables.productImage[0];
          fd.append('ProductImage', file.originFileObj, file.originFileObj.name);
        } else {
          console.warn('No image file selected for upload.');
        }
        response = await fetch(`${API_URL}/${resource}/Post?${params}`, {
          method: "POST", // Assuming PATCH for product updates
          body: fd,
          headers: {
            ...headers,
            Authorization: `Bearer ${localStorage.getItem("access_token")}`,           
          },
        });
      }

      else if(!hasBody){
        Object.entries(variables).forEach(
          ([key, value]) => 
            params.append(key, value)
        );
        response = await fetch(`${API_URL}/${resource}/Post?${params}`, {
          method: "POST",
          headers: {
            ...headers,
            Authorization: `Bearer ${localStorage.getItem("access_token")}`,
          },
        });
      }
      else{
        response = await fetch(`${API_URL}/${resource}/Post`, {
          method: "POST",
          body: JSON.stringify(variables),
          headers: {
            ...headers,
            Authorization: `Bearer ${localStorage.getItem("access_token")}`,
          },
        });
      }
      
  
      if (response.status < 200 || response.status > 299) throw response;
  
      const data = await response.json();
      const extractedData = data.data;

      return { data: extractedData };
    },
    deleteOne: async ({ resource, id }) => {
      const response = await fetch(`${API_URL}/${resource}/delete/${id}`,{
        method: "DELETE",      
        headers: {
          Authorization: `Bearer ${localStorage.getItem("access_token")}`,
        }    
      });

      if (response.status < 200 || response.status > 299) throw response;

      const data = await response.json();

      return { data };    
    },
    getApiUrl: () => API_URL,
};