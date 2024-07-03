import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './Lesson.css';
import FilesModal from './FilesModal';
import Homework from '../Homework/Homework';
import { useAuth } from '../../context/AuthContext';

const Lesson = ({ lesson, refreshLessons }) => {
  const [studentName, setStudentName] = useState('');
  const [showModalToUpload, setShowModalToUpload] = useState(false);
  const [filesType, setFilesType] = useState(null);
  const [selectedFiles, setSelectedFiles] = useState(null);
  const [isFilesModalOpen, setIsFilesModalOpen] = useState(false);
  const [filesUrls, setFilesUrls] = useState([]);
  const [isHomeworkOpen, setIsHomeworkOpen] = useState(false);
  const [homeworkId, setHomeworkId] = useState(null);

  const [name, setName] = useState(lesson.name || '');
  const [description, setDescription] = useState(lesson.description || '');
  const [paidStatus, setPaidStatus] = useState(lesson.paidStatus || 'Nie');

  const { currentUser } = useAuth();

  useEffect(() => {
    const getUserName = async () => {
      try {
        const response = await axios.get(`/api/users/id/${lesson.studentId}`);
        setStudentName(response.data.firstName + " " + response.data.lastName);
        //setName(lesson.name)
        //setDescription(lesson.description)
        //setPaidStatus(lesson.paidStatus)
      } catch (error) {
        console.error('Błąd zwracania ID użytkownika', error);
        setStudentName('Nieznany Uczeń');
      }
    };

    if (lesson.studentId) {
      getUserName();
    }
  }, [lesson.studentId]);


  const handleFilesChange = (e) => {
    setSelectedFiles(e.target.files);
  };

  const showModalToUploadFunction = (type) => {
    setFilesType(type);
    setShowModalToUpload(true);
  };


  const handleUpload = async (e) => {
    e.preventDefault();
    if (!selectedFiles.length || !lesson.id) return;

    const formData = new FormData();
    Array.from(selectedFiles).forEach(file => {
      formData.append('files', file);
    });

    try {
      const response = await axios.post(`/api/lessons/${lesson.id}/upload-${filesType}`, formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
          Authorization: `Bearer ${localStorage.getItem('token')}`,
        },
      });

      console.log('Materials uploaded successfully:', response.data);
      setShowModalToUpload(false);
    } catch (error) {
      console.error('Error uploading materials:', error);
    }
  };

  const handleViewFiles = async (lessonId, type) => {
    try {
      const response = await axios.get(`/api/lessons/${lessonId}/${type}s`, {
        headers: { Authorization: `Bearer ${localStorage.getItem('token')}` },
      });
      setFilesUrls(response.data.$values);
      setIsFilesModalOpen(true);
    } catch (error) {
      console.error('Error fetching notes', error);
      alert('Brak dostępnych plików do wyświetlenia.')
    }
  };

  const showHomeworkFunction = async () => {
    try {
      const response = await axios.get(`/api/homeworks/by-lesson/${lesson.id}`, {
        headers: { Authorization: `Bearer ${localStorage.getItem('token')}` },
      });
      setHomeworkId(response.data.id);
      setIsHomeworkOpen(true);
    } catch (error) {
      console.error('Error fetching HomeworkId:', error);
    }
  }

  const handleSaveChanges = async () => {
    try {
      const updatedLesson = {
        id: lesson.id,
        description: description,
        paidStatus: paidStatus
      };
      await axios.put(`/api/lessons/${lesson.id}`, updatedLesson, {
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${localStorage.getItem('token')}`,
        },
      });
      alert('Zmiany zostały zapisane.');
    } catch (error) {
      console.error('Błąd podczas zapisywania zmian w lekcji', error);
      alert('Nie udało się zapisywać zmian.');
    }
  };

  const handleDeleteLesson = async () => {
    if (window.confirm("Czy na pewno chcesz usunąć tę lekcję?")) {
      try {
        const response = await axios.delete(`/api/lessons/${lesson.id}`, {
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('token')}`,
          },
        });
        console.log("Lekcja została usunięta:", response.data);
        refreshLessons();
      } catch (error) {
        console.error("Błąd podczas usuwania lekcji:", error);
      }
    }
  };

  const formatDate = (dateString) => {
    const options = { year: 'numeric', month: '2-digit', day: '2-digit', hour: '2-digit', minute: '2-digit', hour12: false };
    return new Date(dateString).toLocaleString('default', options).replace(',', '');
  };

  return (
    <div className="lesson">
      {currentUser.role !== "student" && (
        <button className="delete-lesson-button" onClick={handleDeleteLesson}>Usuń lekcję</button>
      )}
      <h3>{name}</h3>
      <label>Uczeń: {studentName}</label>
      <p>Data: {formatDate(lesson.date)}</p>
      <div className="paid-status">
        <span>Opłacona:</span>
        <select value={paidStatus} onChange={(e) => setPaidStatus(e.target.value)} disabled = {currentUser.role === "student"}>
          <option value="Tak">Tak</option>
          <option value="Nie">Nie</option>
        </select>
      </div>
      <p>Praca domowa: {lesson.homework.status}</p>
      <label>Opis</label>
      <textarea
        className="lesson-description"
        value={description}
        onChange={(e) => setDescription(e.target.value) } 
        disabled = {currentUser.role === "student"}
      />
      {currentUser.role !== "student" && (
        <button onClick={handleSaveChanges}>Zapisz zmiany</button>
      )}

      <div className="buttons-container">
        <div className="button yellow" onClick={() => showModalToUploadFunction('material')}>Dodaj materiały</div>
        <div className="button yellow" onClick={() => handleViewFiles(lesson.id, 'material')}>Zobacz materiały</div>
        {currentUser.role !== "student" && (
        <div className="button green" onClick={() => showModalToUploadFunction('note')}>Dodaj notatkę</div>
      )}
        <div className="button green" onClick={() => handleViewFiles(lesson.id, 'note')}>Zobacz notatki</div>
        <div className="button blue" onClick={() => showHomeworkFunction()}>Praca Domowa</div>
      </div>
      {
        showModalToUpload && (
          <div className="upload-modal">
            <form onSubmit={handleUpload}>
              <input type="file" multiple onChange={handleFilesChange} />
              <button type="submit">Prześlij</button>
              <button onClick={() => setShowModalToUpload(false)}>Anuluj</button>
            </form>
          </div>
        )
      }

      <FilesModal
        isOpen={isFilesModalOpen}
        filesUrls={filesUrls}
        onClose={() => setIsFilesModalOpen(false)}
      />

      <Homework
        isOpen={isHomeworkOpen}
        homeworkId={homeworkId}
        onClose={() => setIsHomeworkOpen(false)}
      />

    </div>
  );
};

export default Lesson;
