import React, { useState, useEffect } from 'react';
import './AddLesson.css';
import axios from 'axios';
import { useAuth } from '../../context/AuthContext';

const AddLesson = ({ closeForm, fetchLessons }) => {
    const [date, setDate] = useState('');
    const [name, setName] = useState('');
    const [description, setDescription] = useState('');
    const [isPaid, setIsPaid] = useState('Nie');
    const [studentId, setStudentId] = useState('');
    const { currentUser } = useAuth();
    const [students, setStudents] = useState([]);

    useEffect(() => {
        const fetchStudents = async () => {
            try {
                const response = await axios.get(`/api/teacherstudents/teacher/${currentUser.id}/students`);
                console.log(response.data)
                setStudents(response.data.$values); 
            } catch (error) {
                console.error('Błąd przy pobieraniu listy uczniów:', error);
            }
        };

        fetchStudents();
    }, [currentUser.id]);

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const teacherId = currentUser.id;
            const response = await axios.post('/api/lessons/add', {
                date: date,
                name: name,
                description: description,
                paidStatus: isPaid,
                teacherId: teacherId,
                studentId: studentId,
                headers: { Authorization: `Bearer ${localStorage.getItem('token')}` },
            });

            console.log('Lekcja dodana:', response.data);
            closeForm();
            fetchLessons();
        } catch (error) {
            console.error('Błąd przy dodawaniu lekcji:', error);
        }
        closeForm();
    };

    return (
        <div className="add-lesson-overlay">
            <div className="add-lesson-form">
                <form onSubmit={handleSubmit}>
                    <div className="form-group">
                        <label htmlFor="lesson-date">Data lekcji:</label>
                        <input
                            type="datetime-local"
                            id="lesson-date"
                            value={date}
                            onChange={(e) => setDate(e.target.value)}
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="lesson-name">Nazwa lekcji:</label>
                        <textarea
                            id="lesson-name"
                            value={name}
                            onChange={(e) => setName(e.target.value)}
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="lesson-description">Opis lekcji:</label>
                        <textarea
                            id="lesson-description"
                            value={description}
                            onChange={(e) => setDescription(e.target.value)}
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="lesson-paid">Opłacona:</label>
                        <select
                            id="lesson-paid"
                            value={isPaid}
                            onChange={(e) => setIsPaid(e.target.value)}
                        >
                            <option value="Tak">Tak</option>
                            <option value="Nie">Nie</option>
                        </select>
                    </div>
                    <div className="form-group">
                        <label htmlFor="student-id">Uczeń:</label>
                        <select
                            id="student-id"
                            value={studentId}
                            onChange={(e) => setStudentId(e.target.value)}
                        >
                            <option value="">Wybierz ucznia</option>
                            {students.map(student => (
                                <option key={student.id} value={student.id}>
                                    {student.firstName} {student.lastName}
                                </option>
                            ))}
                        </select>
                    </div>
                    <button type="submit">Dodaj Lekcję</button>
                </form>
                <button onClick={closeForm}>Anuluj</button>
            </div>
        </div>
    );
};

export default AddLesson;
