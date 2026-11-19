import api from "./api";
import type { JobsPage } from "../types";

interface GetJobsParams {
  page: number;
  pageSize: number;
  search?: string;
  signal?: AbortSignal;
}

export default class JobService 
{
  async getJobs({page, pageSize, search, signal}: GetJobsParams): Promise<JobsPage> {
    const response = await api.get<JobsPage>("/jobs", {
      params: {
        page,
        pageSize,
        ...(search ? { search } : {}),
      },
      signal,
    });

    return response.data;
  } 
};
