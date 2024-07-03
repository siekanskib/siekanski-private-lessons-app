import React, { createContext, useState, useContext, useEffect } from 'react';

const AuthContext = createContext(null);

export const useAuth = () => useContext(AuthContext);

const parseUserDataFromToken = (token) => {
  const base64Url = token.split('.')[1];
  const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
  const payload = JSON.parse(window.atob(base64));
  return {
    id: payload.sub,
    role: payload.role
  };
};

export const AuthProvider = ({ children }) => {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [currentUser, setCurrentUser] = useState(null);
  useEffect(() => {
    const token = localStorage.getItem('token');
    console.log("Pobrany token:", token);
    if (token) {
      const userData = parseUserDataFromToken(token);
      setCurrentUser(userData);
    }

    setIsLoggedIn(!!token);
  }, []);


  const login = (token) => {
    localStorage.setItem('token', token);
    const userData = parseUserDataFromToken(token); 
    setCurrentUser(userData); 
    setIsLoggedIn(true);
  };


  const logout = () => {
    localStorage.removeItem('token');
    setCurrentUser(null);
    setIsLoggedIn(false);
  };


  return (
    <AuthContext.Provider value={{ isLoggedIn, currentUser, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

