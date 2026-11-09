import { useState } from 'react';
import { api } from '../api/client';
import ResultCard from '../components/ResultCard';
import { SAMPLES } from '../data/samples';

const MIN_LENGTH = 10;
const MAX_LENGTH = 5000;

export default function CheckPage() {
  const [text, setText] = useState('');
  const [result, setResult] = useState(null);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const canSubmit = text.trim().length >= MIN_LENGTH && !loading;

  function applySample(sampleText) {
    setText(sampleText);
    setResult(null);
    setError('');
  }

  async function handleSubmit(event) {
    event.preventDefault();
    setError('');
    setResult(null);
    setLoading(true);
    try {
      const data = await api.analyze(text.trim());
      setResult(data);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }

  return (
    <div>
      <h1 className="page-title">Check a job offer</h1>
      <p className="muted">
        Paste a recruiter message, email or job post. RedFlag looks for common scam signs and
        explains what it finds.
      </p>

      <form className="card" onSubmit={handleSubmit}>
        <div className="samples">
          <span className="muted small">Try a sample:</span>
          {SAMPLES.map((sample) => (
            <button
              key={sample.label}
              type="button"
              className="btn-secondary"
              onClick={() => applySample(sample.text)}
            >
              {sample.label}
            </button>
          ))}
        </div>

        <label htmlFor="message" className="label">
          Message
        </label>
        <textarea
          id="message"
          rows={8}
          maxLength={MAX_LENGTH}
          value={text}
          onChange={(event) => setText(event.target.value)}
          placeholder="Paste the message here..."
        />
        <div className="counter">
          {text.length} / {MAX_LENGTH}
        </div>
        {error && <p className="error">{error}</p>}
        <button type="submit" className="btn" disabled={!canSubmit}>
          {loading ? 'Checking…' : 'Check message'}
        </button>
        <p className="muted small">
          Privacy: phone numbers, emails, Aadhaar and PAN numbers are hidden before a message is
          saved.
        </p>
      </form>

      {result && <ResultCard result={result} />}
    </div>
  );
}