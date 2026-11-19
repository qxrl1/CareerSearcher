import { useEffect } from "react";
import CompanyMark from "../CompanyMark/CompanyMark";
import type { Job } from "../../types";
import "./JobDetailsModal.css";

interface JobDetailsModalProps {
  job: Job;
  onClose: () => void;
}

function JobDetailsModal({ job, onClose }: JobDetailsModalProps) {
  useEffect(() => {
    function closeOnEscape(event: KeyboardEvent) {
      if (event.key === "Escape") {
        onClose();
      }
    }

    window.addEventListener("keydown", closeOnEscape);
    return () => window.removeEventListener("keydown", closeOnEscape);
  }, [onClose]);

  return (
    <div
      className="modal-backdrop"
      onMouseDown={(event) => {
        if (event.target === event.currentTarget) {
          onClose();
        }
      }}
    >
      <section
        className="job-modal"
        role="dialog"
        aria-modal="true"
        aria-labelledby="modal-title"
      >
        <button
          className="modal-close"
          type="button"
          aria-label="Close job details"
          onClick={onClose}
        >
          ×
        </button>
        <CompanyMark company={job.company} size="large" />
        <p className="modal-company">{job.company}</p>
        <h2 id="modal-title">{job.name}</h2>
        <div className="modal-divider" />
        <h3>About this role</h3>
        <p className="modal-description">
          {job.description || "No description provided."}
        </p>
        {job.createdAt && (
          <p className="posted-date">
            Added{" "}
            {new Date(job.createdAt).toLocaleDateString(undefined, {
              year: "numeric",
              month: "long",
              day: "numeric",
            })}
          </p>
        )}
        <a
          className="apply-button"
          href={job.url}
          target="_blank"
          rel="noreferrer"
        >
          View original listing <span aria-hidden="true">↗</span>
        </a>
      </section>
    </div>
  );
}

export default JobDetailsModal;
