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
        const { headers, hasBody } = meta;

        let response;
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
  
      const data = await response.json();
      const extractedData = data.data;
      
      return {
        data: extractedData,
        total: 0, 
      };
    },
    create: async ({ resource, variables, meta }) => {
      const { headers, hasBody } = meta;

      const params = new URLSearchParams();
      let response;
      if(!hasBody){
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