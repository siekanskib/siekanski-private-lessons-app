import { useAuth } from '../context/AuthContext';
import React, { useState, useEffect } from 'react';
import './StudentManager.css';
import axios from 'axios';
import StudentCard from '../components/StudentCard/StudentCard';

const StudentManager = () => {
    const { currentUser } = useAuth();
    const [Email, setEmail] = useState('');
    const [students, setStudents] = useState([]);
    const [error, setError] = useState('');

    const handleEmailChange = (event) => {
        setEmail(event.target.value);
        setError('');
    };

    const fetchStudents = async () => {
        try {
            const response = await axios.get(`/api/teacherstudents/teacher/${currentUser.id}/students`);
            setStudents(response.data.$values);
        } catch (error) {
            console.error('Błąd przy pobieraniu listy uczniów:', error);
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');

        try {
            const studentResponse = await axios.get(`/api/users/email/${Email}`);
            const studentId = studentResponse.data.id
            console.log(studentId)
            const teacherId = currentUser.id;
            console.log(teacherId)
            const response = await axios.post('/api/teacherstudents/add', {
                teacherId, studentId
            });
            console.log('Uczeń dodany:', response.data);
            fetchStudents();
        } catch (error) {
            if (error.response) {
                setError(error.response.data);
            } else {
                setError('Wystąpił problem podczas dodawania ucznia.');
            }
            console.error('Błąd przy dodawaniu studenta:', error);
        }

    };
    
    useEffect(() => {
        if (currentUser.role === 'teacher') {
            fetchStudents();
        }
    }, [currentUser]);

    return (
        <div className="add-student-container">
            <div className="add-student-form">
                <form onSubmit={handleSubmit}>
                    <h2>Dodaj nowego ucznia</h2>
                    <div className="input-group">
                        <input type="text" id="email" placeholder="Wpisz email ucznia" value={Email} onChange={handleEmailChange} />
                        {error && <p className="error-message">{error}</p>}
                    </div>
                    <button type="submit">Dodaj</button>
                </form>
            </div>

            {students.map(student => (              
                <StudentCard key={student.id} student={student} />
                
            ))}
        </div>
    );
}

export default StudentManager;