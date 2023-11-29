import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";

const baseUrl = import.meta.env.BASE_URL;

export const api = createApi({
	baseQuery: fetchBaseQuery({ baseUrl }),
	endpoints: () => ({}),
});
