import React, { useState, useEffect } from 'react';
import './Navbar.css';
import axios from 'axios';
import { useAuth } from '../../context/AuthContext';

const Navbar = ({ onAddClick, onStudentSelect, onIsPaidSelect, onHomeworkSelect }) => {
  const [students, setStudents] = useState([]);
  const { currentUser } = useAuth();
  const [studentId, setStudentId] = useState('')
  const [isPaidFilter, setIsPaidFilter] = useState('')
  const [homeworkFilter, setHomeworkFilter] = useState('')

  useEffect(() => {
    const fetchStudents = async () => {
      try {
        const response = await axios.get(`/api/teacherstudents/teacher/${currentUser.id}/students`);
        setStudents(response.data.$values);
      } catch (error) {
        console.error('Błąd przy pobieraniu listy uczniów:', error);
      }
    };
    if (currentUser) {
      fetchStudents();
    }
  }, [currentUser?.id]);

  if (!currentUser) {
    return null;
  }

  return (
    <div className="navbar">
      {currentUser.role !== "student" && (
        <button onClick={onAddClick}>Dodaj lekcję</button>
      )}
      {currentUser.role !== "student" && (
        <>
          <label htmlFor="student-id">Uczeń:</label>
          <select
            id="student-id"
            value={studentId}
            onChange={(e) => {
              onStudentSelect(e.target.value);
              setStudentId(e.target.value);
            }}
          >
            <option value="">Wszyscy uczniowie</option>
            {students.map(student => (
              <option key={student.id} value={student.id}>
                {student.firstName} {student.lastName}
              </option>
            ))}
          </select>
        </>
      )}
      <label htmlFor="isPaidFilter">Status płatności lekcji:</label>
      <select
        id="isPaidFilter"
        value={isPaidFilter}
        onChange={(e) => onIsPaidSelect(e.target.value) & setIsPaidFilter(e.target.value)}
      >
        <option value="">Wszystkie</option>
        <option value="Tak">Opłacone</option>
        <option value="Nie">Nieopłacone</option>
      </select>
      <label htmlFor="homeworkFilter">Status pracy domowej:</label>
      <select
        id="homeworkFilter"
        value={homeworkFilter}
        onChange={(e) => onHomeworkSelect(e.target.value) & setHomeworkFilter(e.target.value)}
      >
        <option value="">Wszystkie</option>
        <option value="Brak">Brak</option>
        <option value="Do zrobienia">Do zrobienia</option>
        <option value="Do sprawdzenia">Do sprawdzenia</option>
        <option value="Sprawdzona">Sprawdzona</option>
      </select>
    </div>
  );
};

export default Navbar;

