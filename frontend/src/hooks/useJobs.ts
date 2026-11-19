import { useEffect, useState } from "react";
import JobService from "../services/jobService";
import type { JobsPage } from "../types";

const pageSize = 10;
const jobService = new JobService();

export function useJobs() {
  const [search, setSearch] = useState("");
  const [debouncedSearch, setDebouncedSearch] = useState("");
  const [page, setPage] = useState(1);
  const [results, setResults] = useState<JobsPage | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState("");
  const [reloadKey, setReloadKey] = useState(0);

  useEffect(() => {
    const timeout = window.setTimeout(() => {
      setDebouncedSearch(search.trim());
    }, 250);

    return () => window.clearTimeout(timeout);
  }, [search]);

  useEffect(() => {
    const controller = new AbortController();

    async function loadJobs() {
      setIsLoading(true);
      setError("");

      try {
        const data = await jobService.getJobs({
          page,
          pageSize,
          search: debouncedSearch,
          signal: controller.signal,
        });

        setResults(data);
      } catch (requestError) {
        if (controller.signal.aborted)
          return;

        setError(requestError instanceof Error ? requestError.message : "Something went wrong while loading jobs.");
        setResults(null);
      } finally {
        if (!controller.signal.aborted)
          setIsLoading(false);
      }
    }

    void loadJobs();
    return () => controller.abort();
  }, [page, debouncedSearch, reloadKey]);

  function updateSearch(value: string) {
    setSearch(value);
    setPage(1);
  }

  function retry() {
    setReloadKey((key) => key + 1);
  }

  return {search, updateSearch, page, setPage, results, isLoading, error, retry};
}
