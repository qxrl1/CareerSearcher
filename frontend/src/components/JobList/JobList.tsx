import JobCard from "../JobCard/JobCard";
import JobPagination from "../JobPagination/JobPagination";
import type { Job } from "../../types";
import "./JobList.css";

interface JobListProps {
  jobs: Job[];
  search: string;
  page: number;
  pageSize: number;
  totalJobs: number;
  totalPages: number;
  isLoading: boolean;
  error: string;
  onRetry: () => void;
  onPageChange: (page: number) => void;
  onSelectJob: (job: Job) => void;
}

function JobList({
  jobs,
  search,
  page,
  pageSize,
  totalJobs,
  totalPages,
  isLoading,
  error,
  onRetry,
  onPageChange,
  onSelectJob,
}: JobListProps) {
  if (error) {
    return (
      <div className="message message-error" role="alert">
        <p>{error}</p>
        <button className="text-button" type="button" onClick={onRetry}>
          Try again
        </button>
      </div>
    );
  }

  if (isLoading) {
    return (
      <div className="loading-state" role="status">
        <span className="spinner" aria-hidden="true" />
        <span>Loading jobs...</span>
      </div>
    );
  }

  if (jobs.length === 0) {
    return (
      <div className="empty-state">
        <span className="empty-icon" aria-hidden="true">
          ↗
        </span>
        <h3>No jobs found</h3>
        <p>
          {search
            ? "Try another title, company, or description."
            : "New opportunities will show up here."}
        </p>
      </div>
    );
  }

  return (
    <>
      <div className="job-list">
        {jobs.map((job) => (
          <JobCard key={job.uniqueId} job={job} onSelect={onSelectJob} />
        ))}
      </div>
      <JobPagination
        page={page}
        pageSize={pageSize}
        totalJobs={totalJobs}
        totalPages={totalPages}
        onPageChange={onPageChange}
      />
    </>
  );
}

export default JobList;
