export interface Job {
  name: string;
  company: string;
  description: string;
  url: string;
  uniqueId: string;
  createdAt: string;
}

export interface JobsPage {
  page: number;
  pageSize: number;
  amount: number;
  pages: number;
  totalInPage: number;
  data: Job[];
}
