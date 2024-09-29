export const getTasks = () => {
  return useNuxtApp().$axiosApiClient.get("/api/Tasks");
};

export const getTask = (id) => {
  return useNuxtApp().$axiosApiClient.get(`/api/Tasks/${id}`);
};

export const createTask = (record) => {
  return useNuxtApp().$axiosApiClient.post("/api/Tasks", record);
};

export const updateTask = (id, record) => {
  return useNuxtApp().$axiosApiClient.put(`/api/Tasks/${id}`, record);
};

export const deleteTask = (id) => {
  return useNuxtApp().$axiosApiClient.delete(`/api/Tasks/${id}`);
};

export const assignTagsToTask = (id, data) => {
  return useNuxtApp().$axiosApiClient.post(`/api/Tasks/${id}/tags`, data);
};
