import { useCallback, useState } from "react";
import "./App.css";
import JobDetailsModal from "./components/JobDetailsModal/JobDetailsModal";
import JobList from "./components/JobList/JobList";
import JobSearch from "./components/JobSearch/JobSearch";
import { useJobs } from "./hooks/useJobs";
import type { Job } from "./types";

function App() {
  const {
    search,
    updateSearch,
    page,
    setPage,
    results,
    isLoading,
    error,
    retry,
  } = useJobs();
  const [selectedJob, setSelectedJob] = useState<Job | null>(null);
  const closeSelectedJob = useCallback(() => setSelectedJob(null), []);

  return (
    <div className="app-shell">
      <main className="main-content">
        <section className="jobs-panel" aria-label="Job listings">
          <JobSearch
            search={search}
            totalJobs={results?.amount ?? 0}
            isLoading={isLoading}
            onSearchChange={updateSearch}
          />
          <JobList
            jobs={results?.data ?? []}
            search={search}
            page={page}
            pageSize={results?.pageSize ?? 10}
            totalJobs={results?.amount ?? 0}
            totalPages={results?.pages ?? 0}
            isLoading={isLoading}
            error={error}
            onRetry={retry}
            onPageChange={setPage}
            onSelectJob={setSelectedJob}
          />
        </section>
      </main>

      {selectedJob && (
        <JobDetailsModal job={selectedJob} onClose={closeSelectedJob} />
      )}
    </div>
  );
}

export default App;
