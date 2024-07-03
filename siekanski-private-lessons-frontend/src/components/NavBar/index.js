import {React, useState} from 'react'
import { Nav, NavLink, Bars, NavMenu, NavBtnLink } from './NavBarElements';
import { useAuth } from '../../context/AuthContext';

const NavBar = () => {
  const { isLoggedIn, logout, currentUser } = useAuth();
  const [isOpen, setIsOpen] = useState(false);

  const toggle = () => {
    setIsOpen(!isOpen);
  };

  return (
    <Nav>
      <NavLink to="/">
        <h1>Siekanski Private Lessons</h1>
      </NavLink>
      <Bars onClick={toggle} />
      <NavMenu isOpen={isOpen}>
        {isLoggedIn ? (
          <>
            {currentUser.role !== "student" && (
              <NavBtnLink to="/studentmanager" onClick={toggle}>Zarządzaj uczniami</NavBtnLink>
            )}
            <NavBtnLink to="/mylessons" onClick={toggle}>Moje lekcje</NavBtnLink>
            <NavBtnLink to="/" onClick={logout}>Wyloguj się</NavBtnLink>
          </>
        ) : (
          <>
            <NavBtnLink to="/signin" onClick={toggle}>Zaloguj się </NavBtnLink>
            <NavBtnLink to="/signup" onClick={toggle}>Zarejestruj się</NavBtnLink>
          </>
        )}

      </NavMenu>
    </Nav>
  );
}

export default NavBar
