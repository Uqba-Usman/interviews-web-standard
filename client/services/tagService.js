export const getTags = () => {
  return useNuxtApp().$axiosApiClient.get("/api/Tags");
};

export const getTag = (id) => {
  return useNuxtApp().$axiosApiClient.get(`/api/Tags/${id}`);
};

export const createTag = (record) => {
  return useNuxtApp().$axiosApiClient.post("/api/Tags", record);
};

export const updateTag = (id, record) => {
  return useNuxtApp().$axiosApiClient.put(`/api/Tags/${id}`, record);
};

export const deleteTag = (id) => {
  return useNuxtApp().$axiosApiClient.delete(`/api/Tags/${id}`);
};
