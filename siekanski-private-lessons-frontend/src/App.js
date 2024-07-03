import './App.css';
import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import NavBar from './components/NavBar';
import Home from './pages';
import SignIn from './pages/SignIn';
import SignUp from './pages/SignUp';
import MyLessons from './pages/MyLessons';
import StudentManager from './pages/StudentManager';
import { AuthProvider } from './context/AuthContext';

function App() {
  return (
      
    <Router>
      <AuthProvider>
        <NavBar />
        <Routes>
          <Route path="/" exact element={<Home />} />
          <Route path="/signin" element={<SignIn />} />
          <Route path="/signup" element={<SignUp />} />
          <Route path="/mylessons" element={<MyLessons />} />
          <Route path="/studentmanager" element={<StudentManager />} />
        </Routes>
      </AuthProvider>
    </Router>

  );
}

export default App;

