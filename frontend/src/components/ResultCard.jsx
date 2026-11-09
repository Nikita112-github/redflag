import { Link } from 'react-router-dom';
import { levelClass } from '../utils/format';

const ADVICE = {
  Low: 'No common scam signs were found. Still verify the employer through official channels.',
  Medium:
    'Some warning signs were found. Be careful and verify the employer before paying or sharing documents.',
  High: 'Several strong warning signs were found. Do not pay money or share documents until you have verified the employer.',
};

export default function ResultCard({ result, showLink = true }) {
  const level = result.level;
  const cls = levelClass(level);

  return (
    <section className={`card result result-${cls}`} aria-live="polite">
      <div className="result-header">
        <div>
          <div className="muted small">Risk level</div>
          <span className={`badge badge-${cls}`}>{level}</span>
        </div>
        <div>
          <span className="score-number">{result.score}</span>
          <span className="muted"> / 100</span>
        </div>
      </div>

      <div
        className="meter"
        role="progressbar"
        aria-valuenow={result.score}
        aria-valuemin={0}
        aria-valuemax={100}
      >
        <div className={`meter-fill meter-${cls}`} style={{ width: `${result.score}%` }} />
      </div>

      <p className="advice">{ADVICE[level]}</p>

      {result.flags.length === 0 ? (
        <p className="muted">No warning signs were triggered.</p>
      ) : (
        <>
          <h3>Why this score</h3>
          <ul className="flag-list">
            {result.flags.map((flag) => (
              <li key={flag.code} className="flag">
                <div className="flag-title">
                  <code>{flag.code}</code>
                  <span className="muted small">+{flag.weight}</span>
                </div>
                <p>{flag.explanation}</p>
              </li>
            ))}
          </ul>
        </>
      )}

      {level !== 'Low' && (
        <p className="advice-help">
          If you have been targeted by a job scam, report it on the National Cyber Crime Reporting
          Portal or call 1930.
        </p>
      )}

      <p className="disclaimer">{result.disclaimer}</p>

      {showLink && result.reportId && <Link to={`/report/${result.reportId}`}>Open saved result</Link>}
    </section>
  );
}