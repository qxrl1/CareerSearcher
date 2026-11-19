import "./CompanyMark.css";

interface CompanyMarkProps {
  company: string;
  size?: "default" | "large";
}

function CompanyMark({ company, size = "default" }: CompanyMarkProps) {
  const sizeClass = size === "large" ? "company-mark-large" : "";

  return (
    <div className={`company-mark ${sizeClass}`} aria-hidden="true">
      {company.trim().charAt(0).toUpperCase() || "J"}
    </div>
  );
}

export default CompanyMark;
