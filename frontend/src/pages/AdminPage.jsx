import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { api } from '../api/client';
import { useAuth } from '../auth/useAuth';
import { formatDateTime, levelClass, truncate } from '../utils/format';

export default function AdminPage() {
  const { token, logout } = useAuth();
  const [state, setState] = useState({ status: 'loading', stats: null, reports: [], error: '' });

  useEffect(() => {
    let cancelled = false;

    Promise.all([api.getStats(token), api.getLatestReports(token, 10)])
      .then(([stats, reports]) => {
        if (!cancelled) {
          setState({ status: 'done', stats, reports, error: '' });
        }
      })
      .catch((err) => {
        if (cancelled) return;
        if (err.status === 401) {
          logout();
          return;
        }
        setState({ status: 'error', stats: null, reports: [], error: err.message });
      });

    return () => {
      cancelled = true;
    };
  }, [token, logout]);

  if (state.status === 'loading') {
    return <p className="muted">Loading…</p>;
  }

  if (state.status === 'error') {
    return (
      <div className="card">
        <p className="error">{state.error}</p>
      </div>
    );
  }

  const { stats, reports } = state;

  return (
    <div>
      <h1 className="page-title">Admin dashboard</h1>
      <p className="muted">Message text is shown with personal details already hidden.</p>

      <div className="stats-grid">
        <div className="card">
          <div className="muted small">Total reports</div>
          <div className="stat-number">{stats.totalReports}</div>
        </div>
        {stats.byLevel.map((item) => (
          <div key={item.level} className="card">
            <div className="muted small">{item.level} risk</div>
            <div className="stat-number">{item.count}</div>
          </div>
        ))}
      </div>

      <div className="card">
        <h2>Most common warning signs</h2>
        {stats.topFlags.length === 0 ? (
          <p className="muted">No data yet.</p>
        ) : (
          <div className="table-wrap">
            <table>
              <thead>
                <tr>
                  <th>Rule</th>
                  <th>Times triggered</th>
                </tr>
              </thead>
              <tbody>
                {stats.topFlags.map((flag) => (
                  <tr key={flag.code}>
                    <td>
                      <code>{flag.code}</code>
                    </td>
                    <td>{flag.count}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>

      <div className="card">
        <h2>Latest reports</h2>
        {reports.length === 0 ? (
          <p className="muted">No reports yet.</p>
        ) : (
          <div className="table-wrap">
            <table>
              <thead>
                <tr>
                  <th>#</th>
                  <th>Time</th>
                  <th>Level</th>
                  <th>Score</th>
                  <th>Message (masked)</th>
                  <th>Flags</th>
                </tr>
              </thead>
              <tbody>
                {reports.map((report) => (
                  <tr key={report.id}>
                    <td>
                      <Link to={`/report/${report.id}`}>{report.id}</Link>
                    </td>
                    <td>{formatDateTime(report.createdAt)}</td>
                    <td>
                      <span className={`badge badge-${levelClass(report.level)}`}>{report.level}</span>
                    </td>
                    <td>{report.score}</td>
                    <td>{truncate(report.maskedText, 80)}</td>
                    <td>{report.flags.map((flag) => flag.code).join(', ') || '—'}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </div>
  );
}