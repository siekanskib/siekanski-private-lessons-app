import React, { useState } from 'react';
import './SignUp.css';
import axios from 'axios';

const SignUp = () => {
    const [formData, setFormData] = useState({
        FirstName: '',
        LastName: '',
        Username: '',
        Email: '',
        Password: '',
        Role: ''

    });

    const [error, setError] = useState('');
    const [errorUserName, setErrorUserName] = useState('');

    const handleChange = (e) => {
        const { id, value, type, name } = e.target;
        if (type === "radio") {
            setFormData({ ...formData, [name]: value });
        } else {
            setFormData({ ...formData, [id]: value });
        }
        setError('');
        setErrorUserName('');
    };


    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const response = await axios.post('/api/users/register', formData);
        } catch (error) {
            console.error(error); 
            if (error.response && error.response.status === 400 && error.response.data?.message?.includes('email')) {
                setError('Użytkownik o podanym adresie e-mail już istnieje.');
            } else if (error.response && error.response.status === 400 && error.response.data?.message?.includes('użytkownika')) {
                setErrorUserName('Nazwa użytkownika jest zajęta.');
            }
        }
    };

    return (
        <div className="signup-container">
            <div className="signup-box"> 
                <h2>Rejestracja</h2>
                <form onSubmit={handleSubmit}>
                    <div className="input-group">
                        <label htmlFor="FirstName">Imię</label>
                        <input type="text" id="FirstName" placeholder="Wpisz swoje imię" required pattern="[A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ]+" title="Wprowadź poprawne imię" value={formData.FirstName} onChange={handleChange} />
                    </div>
                    <div className="input-group">
                        <label htmlFor="LastName">Nazwisko</label>
                        <input type="text" id="LastName" placeholder="Wpisz swoje nazwisko" required pattern="[A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ\-]+" title="Wprowadź poprawne nazwisko" value={formData.LastName} onChange={handleChange} />
                    </div>
                    <div className="input-group">
                        <label htmlFor="Username">Nazwa użytkownika</label>
                        <input type="text" id="Username" placeholder="Wpisz nazwę użytkownika" value={formData.Username} required onChange={handleChange} />
                    </div>
                    {errorUserName && <p className="error-message">{errorUserName}</p>} 
                    <div className="input-group">
                        <label htmlFor="Email">Email</label>
                        <input type="email" id="Email" placeholder="Wpisz swój email" value={formData.Email} required onChange={handleChange} />
                    </div>
                    {error && <p className="error-message">{error}</p>} 
                    <div className="input-group">
                        <label htmlFor="Password">Hasło</label>
                        <input type="password" id="Password" placeholder="Wpisz swoje hasło" value={formData.Password} required minlength="8" onChange={handleChange} />
                    </div>
                    <div className="input-group radio">
                        <label htmlFor="student">Uczeń</label>
                        <input type="radio" id="student" name="Role" value="student" checked={formData.Role === 'student'} required onChange={handleChange} />
                        <label htmlFor="teacher">Nauczyciel</label>
                        <input type="radio" id="teacher" name="Role" value="teacher" checked={formData.Role === 'teacher'} required onChange={handleChange} />
                    </div>
                    <button type="submit">Zarejestruj się</button>
                </form>
                <p style={{ marginTop: "16px", textAlign: "center" }}>
                    Masz już konto? <a href="/signin" style={{ color: "#74b9ff" }}>Zaloguj się</a>
                </p>
            </div>
        </div>
    );
}

export default SignUp;
