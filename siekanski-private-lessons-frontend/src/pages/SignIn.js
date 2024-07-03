import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import './SignIn.css';
import { useAuth } from '../context/AuthContext';

const SignIn = () => {
    const [Username, setUsername] = useState('');
    const [Password, setPassword] = useState('');
    const navigate = useNavigate();
    const [error, setError] = useState('');
    const { login } = useAuth();

    const handleUsernameChange = (event) => {
        setUsername(event.target.value);
        setError('');
    };

    const handlePasswordChange = (event) => {
        setPassword(event.target.value);
        setError('');
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        try {
            const response = await axios.post('/api/users/login', { Username, Password });
            login(response.data.token); 
            navigate('/'); 
        } catch (error) {
            console.error('Błąd logowania', error.response);
            if (error.response) {
                setError('Błędna nazwa użytkownika lub hasło.');
            }
        }
    };

    return (
        <div className="signin-container">
            <div className="signin-content">
                <h2>Logowanie</h2>
                <form onSubmit={handleSubmit}>
                    <div className="input-group">
                        <label htmlFor="username">Nazwa użytkownika</label>
                        <input type="text" id="username" placeholder="Wpisz nazwę użytkownika" value={Username} onChange={handleUsernameChange} />
                    </div>
                    <div className="input-group">
                        <label htmlFor="password">Hasło</label>
                        <input type="password" id="password" placeholder="Wpisz hasło" value={Password} onChange={handlePasswordChange} />
                    </div>
                    {error && <p className="error-message">{error}</p>}
                    <button type="submit">Zaloguj</button>
                </form>
                <p style={{ marginTop: "16px", textAlign: "center" }}>
                    Nie masz konta? <a href="/signup" style={{ color: "#74b9ff" }}>Zarejestruj się</a>
                </p>
            </div>
        </div>
    );
}


export default SignIn;
