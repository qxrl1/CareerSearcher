import "./JobPagination.css";

interface JobPaginationProps {
  page: number;
  pageSize: number;
  totalJobs: number;
  totalPages: number;
  onPageChange: (page: number) => void;
}

function JobPagination({
  page,
  pageSize,
  totalJobs,
  totalPages,
  onPageChange,
}: JobPaginationProps) {
  const firstResult = (page - 1) * pageSize + 1;
  const lastResult = Math.min(page * pageSize, totalJobs);

  return (
    <nav className="pagination" aria-label="Job list pages">
      <p>
        Showing <strong>{firstResult}–{lastResult}</strong> of{" "}
        <strong>{totalJobs}</strong>
      </p>
      <div className="pagination-controls">
        <button
          className="page-button"
          type="button"
          aria-label="Previous page"
          disabled={page <= 1}
          onClick={() => onPageChange(page - 1)}
        >
          ← <span>Previous</span>
        </button>
        <span className="page-count">
          Page <strong>{page}</strong> of <strong>{totalPages}</strong>
        </span>
        <button
          className="page-button"
          type="button"
          aria-label="Next page"
          disabled={page >= totalPages}
          onClick={() => onPageChange(page + 1)}
        >
          <span>Next</span> →
        </button>
      </div>
    </nav>
  );
}

export default JobPagination;
