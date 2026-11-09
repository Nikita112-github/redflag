import { useEffect, useState } from 'react';
import { Link, useParams } from 'react-router-dom';
import { api } from '../api/client';
import ResultCard from '../components/ResultCard';
import { formatDateTime } from '../utils/format';

function ReportLoader({ id }) {
  const [state, setState] = useState({ status: 'loading', report: null, error: '' });

  useEffect(() => {
    let cancelled = false;

    api
      .getReport(id)
      .then((report) => {
        if (!cancelled) {
          setState({ status: 'done', report, error: '' });
        }
      })
      .catch((err) => {
        if (!cancelled) {
          const message = err.status === 404 ? 'Report not found.' : err.message;
          setState({ status: 'error', report: null, error: message });
        }
      });

    return () => {
      cancelled = true;
    };
  }, [id]);

  if (state.status === 'loading') {
    return <p className="muted">Loading…</p>;
  }

  if (state.status === 'error') {
    return (
      <div className="card">
        <p className="error">{state.error}</p>
        <Link to="/">Back to the checker</Link>
      </div>
    );
  }

  const { report } = state;

  return (
    <div>
      <h1 className="page-title">Saved result #{report.reportId}</h1>
      <p className="muted">
        Checked {formatDateTime(report.createdAt)} · Source: {report.source}
      </p>
      <ResultCard result={report} showLink={false} />
      <Link to="/">Check another message</Link>
    </div>
  );
}

export default function ReportPage() {
  const { id } = useParams();
  return <ReportLoader key={id} id={id} />;
}