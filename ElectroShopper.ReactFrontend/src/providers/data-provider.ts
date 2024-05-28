import type { DataProvider } from "@refinedev/core";

const API_URL = "https://localhost:7265/api";

export const dataProvider: DataProvider = {
    getOne: async ({ resource, id, meta }) => {
        const response = await fetch(`${API_URL}/${resource}/detail/${id}`);

        if (response.status < 200 || response.status > 299) throw response;

        const data = await response.json();
        const extractedData = data.data;

        return { data: extractedData };
    },
    update: async ({ resource, id, variables }) => {
        const params = new URLSearchParams();

        Object.entries(variables).forEach(
          ([key, value]) => 
            params.append(key, value)
        );
        
        const response = await fetch(`${API_URL}/${resource}/patch/${id}?${params}`, {
          method: "PATCH",
          headers: {
            "Content-Type": "application/json",
          },
        });
    
        if (response.status < 200 || response.status > 299) throw response;
    
        const data = await response.json();
        const extractedData = data.data;
    
        return { data: extractedData };
    },
    getList: async ({ resource, pagination, filters, sorters, meta }) => {
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
    create: async ({ resource, variables }) => {
      const params = new URLSearchParams();
      Object.entries(variables).forEach(
        ([key, value]) => 
          params.append(key, value)
      );
      const response = await fetch(`${API_URL}/${resource}/Post?${params}`, {
        method: "POST",
        headers: {
          "Content-Type": "multipart/form-data",
        },
      });
  
      if (response.status < 200 || response.status > 299) throw response;
  
      const data = await response.json();
      const extractedData = data.data;

      return { data: extractedData };
    },
    deleteOne: () => {
        throw new Error("Not implemented");
    },
    getApiUrl: () => API_URL,
};