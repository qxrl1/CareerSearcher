import CompanyMark from "../CompanyMark/CompanyMark";
import type { Job } from "../../types";
import "./JobCard.css";

interface JobCardProps {
  job: Job;
  onSelect: (job: Job) => void;
}

function JobCard({ job, onSelect }: JobCardProps) {
  return (
    <article className="job-card">
      <CompanyMark company={job.company} />
      <div className="job-details">
        <h3>{job.name}</h3>
        <p className="company-name">{job.company}</p>
        {job.description && <p className="job-summary">{job.description}</p>}
      </div>
      <button
        className="view-button"
        type="button"
        onClick={() => onSelect(job)}
      >
        View more
        <span aria-hidden="true">→</span>
      </button>
    </article>
  );
}

export default JobCard;
