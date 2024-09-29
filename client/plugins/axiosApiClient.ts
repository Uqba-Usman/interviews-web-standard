import axios from 'axios';
  
export default defineNuxtPlugin((nuxtApp) => {
    const runctimeConfig = useRuntimeConfig()
    const axiosApiClient = axios.create()
    axiosApiClient.defaults.baseURL = runctimeConfig.public.apiUrl
    return {
        provide: {
            axiosApiClient: axiosApiClient,
        },
    };
});