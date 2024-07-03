import React, { useState, useEffect } from 'react';
import Lesson from '../components/Lesson/Lesson';
import { useAuth } from '../context/AuthContext';
import AddLesson from '../components/MyLessons/AddLesson';
import Navbar from '../components/MyLessons/Navbar';
import './MyLessons.css';

import axios from 'axios';
const token = localStorage.getItem('token');
if (token) {
  axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
}

const MyLessons = () => {
  const [lessons, setLessons] = useState([]);
  const { currentUser } = useAuth();
  const [showAddLessonPopup, setShowAddLessonPopup] = useState(false);
  const students = [];
  const [selectedStudentId, setSelectedStudentId] = useState('');
  const [isPaidFilter, setIsPaidFilter] = useState('');
  const [isHomeworkFilter, setHomeworkFilter] = useState('');

  const [currentPage, setCurrentPage] = useState(1);
  const [paginationData, setPaginationData] = useState({});

  const handleStudentSelect = studentId => {
    setSelectedStudentId(studentId);
    fetchLessons(1, studentId, isPaidFilter, isHomeworkFilter);
  };

  const handleIsPaidSelect = (value) => {
    setIsPaidFilter(value);
    fetchLessons(1, selectedStudentId, value, isHomeworkFilter);
  };

  const handleHomeworkSelect = (value) => {
    setHomeworkFilter(value);
    fetchLessons(1, selectedStudentId, isPaidFilter, value);
  };

  const fetchLessons = async (pageNumber = 1, studentId = '', isPaid = '', homework = '') => {
    try {
      let url = `/api/lessons`;
      const studentIdParam = studentId ? `studentId=${studentId}` : '';
      const isPaidParam = isPaid ? `isPaid=${isPaid}` : '';
      const isHomeworkParam = homework ? `homework=${homework}` : '';
      let queryParams = [`pageNumber=${pageNumber}`, `pageSize=6`];
      console.log(currentUser.role)
      if (currentUser.role === "teacher") {
        url += `/forTeacher`
        if (studentId) queryParams.push(studentIdParam);
      }
      else {
        url += `/forStudent`;
      }
      console.log(url)
      if (isPaid) queryParams.push(isPaidParam);
      if (homework) queryParams.push(isHomeworkParam);

      if (queryParams.length > 0) {
        url += `?${queryParams.join('&')}`;
      }
      console.log(url)
      const response = await axios.get(url, {
        headers: { Authorization: `Bearer ${localStorage.getItem('token')}` },
      });
      console.log(response)
      if (response && response.data) {
        setLessons(response.data.lessons.$values);
        setPaginationData(response.data.pagination);
      }
    } catch (error) {
      console.error('Error fetching lessons', error);
    }
  };

  useEffect(() => {
    if (currentUser) {
      fetchLessons(currentPage, selectedStudentId, isPaidFilter, isHomeworkFilter);
    }
  }, [currentPage, currentUser?.id]);



  return (
    <div>
      <Navbar
        onAddClick={() => setShowAddLessonPopup(true)}
        students={students}
        onStudentSelect={handleStudentSelect}
        onIsPaidSelect={handleIsPaidSelect}
        onHomeworkSelect={handleHomeworkSelect}
      />
      {showAddLessonPopup && (
        <AddLesson closeForm={() => setShowAddLessonPopup(false)} fetchLessons={fetchLessons} />
      )}
      {lessons.map((lesson) => (
        <Lesson key={lesson.id} lesson={lesson} refreshLessons={fetchLessons} />
      ))}
      <div className="pagination-buttons">
        {[...Array(paginationData.totalPages)].map((e, i) => (
          <button
            key={i}
            onClick={() => setCurrentPage(i + 1)}
            className={`pagination-button ${currentPage === i + 1 ? "active" : ""}`}
          >
            {i + 1}
          </button>
        ))}
      </div>
    </div>

  );
};

export default MyLessons;
