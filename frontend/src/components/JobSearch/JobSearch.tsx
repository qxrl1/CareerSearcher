import type { ChangeEvent } from "react";
import "./JobSearch.css";

interface JobSearchProps {
  search: string;
  totalJobs: number;
  isLoading: boolean;
  onSearchChange: (value: string) => void;
}

function JobSearch({
  search,
  totalJobs,
  isLoading,
  onSearchChange,
}: JobSearchProps) {
  function handleChange(event: ChangeEvent<HTMLInputElement>) {
    onSearchChange(event.currentTarget.value);
  }

  return (
    <div className="list-heading">
      <div>
        <h2>Latest jobs</h2>
        <p>
          {isLoading
            ? "Finding opportunities..."
            : `${totalJobs.toLocaleString()} ${
                totalJobs === 1 ? "opportunity" : "opportunities"
              }`}
        </p>
      </div>
      <label className="search-box">
        <svg
          aria-hidden="true"
          viewBox="0 0 24 24"
          fill="none"
          className="search-icon"
        >
          <circle cx="10.8" cy="10.8" r="6.8" />
          <path d="m16 16 4.2 4.2" />
        </svg>
        <span className="visually-hidden">Search jobs</span>
        <input
          type="search"
          value={search}
          onChange={handleChange}
          placeholder="Search title, company, or description"
        />
      </label>
    </div>
  );
}

export default JobSearch;
