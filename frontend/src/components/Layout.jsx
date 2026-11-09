import { NavLink, Outlet, useNavigate } from 'react-router-dom';
import { useAuth } from '../auth/useAuth';

export default function Layout() {
  const { user, isAdmin, logout } = useAuth();
  const navigate = useNavigate();

  function handleLogout() {
    logout();
    navigate('/');
  }

  return (
    <div className="app">
      <header className="navbar">
        <div className="nav-inner">
          <NavLink to="/" className="brand">
            🚩 RedFlag
          </NavLink>
          <nav className="nav-links">
            <NavLink to="/" end>
              Check a message
            </NavLink>
            {isAdmin && <NavLink to="/admin">Admin</NavLink>}
            {user ? (
              <>
                <span className="nav-user">{user.email}</span>
                <button type="button" className="btn-link" onClick={handleLogout}>
                  Log out
                </button>
              </>
            ) : (
              <>
                <NavLink to="/login">Log in</NavLink>
                <NavLink to="/register">Register</NavLink>
              </>
            )}
          </nav>
        </div>
      </header>

      <main className="container">
        <Outlet />
      </main>

      <footer className="footer">
        RedFlag gives a risk score, not a verdict. Always verify an employer through official
        channels before paying or sharing documents.
      </footer>
    </div>
  );
}