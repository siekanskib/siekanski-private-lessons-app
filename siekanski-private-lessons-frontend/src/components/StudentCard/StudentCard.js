import React from 'react';
import './StudentCard.css';

const StudentCard = ({ student }) => {
    return (
        <div className="student-card">
            <p>Imię: {student.firstName}</p>
            <p>Nazwisko: {student.lastName}</p>
            <p>Email: {student.email}</p>
        </div>
    );
}

export default StudentCard;
