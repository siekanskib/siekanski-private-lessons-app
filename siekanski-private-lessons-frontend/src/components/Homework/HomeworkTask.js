import React, { useState } from 'react';
import './HomeworkTask.css';
import axios from 'axios';
import FilesModal from '../Lesson/FilesModal';
import { useAuth } from '../../context/AuthContext';

const HomeworkTask = ({ index, id, homeworkId, name, contentProp, feedbackProp, refreshTasks }) => {
  const [content, setContent] = useState(contentProp || '');
  const [feedback, setFeedback] = useState(feedbackProp || '');
  const handleContentChange = (e) => setContent(e.target.value);
  const handleFeedbackChange = (e) => setFeedback(e.target.value);
  const [showModalToUpload, setShowModalToUpload] = useState(false);
  const [filesType, setFilesType] = useState(null);
  const [selectedFiles, setSelectedFiles] = useState(null);
  const [isFilesModalOpen, setIsFilesModalOpen] = useState(false);
  const [filesUrls, setFilesUrls] = useState([]);

  const { currentUser } = useAuth();

  const handleSaveChanges = async () => {
    try {
      const updatedTask = {
        id: id,
        homeworkId: homeworkId, 
        name: name,
        content: content,
        feedback: feedback,
      };

      await axios.put(`/api/homeworktasks/${id}`, updatedTask, {
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${localStorage.getItem('token')}`,
        },
      });

      alert('Zadanie zaktualizowane pomyślnie');

      setContent(updatedTask.content)
      setFeedback(updatedTask.feedback)
    } catch (error) {
      console.error('Błąd podczas aktualizacji zadania', error);
      alert('Nie udało się zaktualizować zadania');
    }
  };

  const handleUpload = async (e) => {
    e.preventDefault();
    if (!selectedFiles.length || !id) return;

    const formData = new FormData();
    Array.from(selectedFiles).forEach(file => {
      formData.append('files', file);
    });

    try {
      const response = await axios.post(`/api/homeworktasks/${id}/upload-${filesType}`, formData, {
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

  const handleViewFiles = async (id, type) => {
    try {
      const response = await axios.get(`/api/homeworktasks/${id}/${type}s`, {
        headers: { Authorization: `Bearer ${localStorage.getItem('token')}` },
      });
      setFilesUrls(response.data.$values);
      setIsFilesModalOpen(true);
    } catch (error) {
      console.error('Error fetching notes', error);
      alert('Brak dostępnych plików do wyświetlenia.')
    }
  };

  const handleFilesChange = (e) => {
    setSelectedFiles(e.target.files);
  };

  const showModalToUploadFunction = (type) => {
    setShowModalToUpload(true);
    setFilesType(type);
  };

  const handleDeleteTask = async () => {
    if (window.confirm("Czy na pewno chcesz usunąć to zadanie?")) {
      try {
        const response = await axios.delete(`/api/homeworktasks/${id}`, {
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('token')}`,
          },
        });
        console.log("Zadanie został usunięte:", response.data);
        refreshTasks();
      } catch (error) {
        console.error("Błąd podczas usuwania zadania:", error);
      }
    }
  };

  return (
    <div className="homework-task">
      {currentUser.role !== "student" && (
        <button className="button-delete" onClick={handleDeleteTask}>Usuń zadanie</button>
      )}
      <h4>Zadanie {index + 1}: {name}</h4>
      <label htmlFor="task-content-{id}">Treść zadania</label>
      <textarea
        id="task-content-{id}"
        className="homework-task-input"
        value={content}
        onChange={handleContentChange}
      />
      <label htmlFor="task-feedback-{id}">Feedback</label>
      <textarea
        id="task-feedback-{id}"
        className="homework-task-input"
        value={feedback}
        onChange={handleFeedbackChange}
      />

      {currentUser.role !== "student" && (
        <button className="button-save" onClick={handleSaveChanges}>Zapisz zmiany</button>
      )}

      <div className="buttons-container">
        <div className="buttons-left">
          {currentUser.role !== "student" && (
            <button className="orange" onClick={() => showModalToUploadFunction('content')}>Dodaj treść</button>
          )}
          <button className="orange" onClick={() => handleViewFiles(id, 'content')}>Zobacz Treść</button>
        </div>
        <div className="buttons-center">
          <button className="green" onClick={() => showModalToUploadFunction('solution')}>Dodaj rozwiązanie</button>
          <button className="green" onClick={() => handleViewFiles(id, 'solution')}>Zobacz rozwiązanie</button>
        </div>
        <div className="buttons-right">
        {currentUser.role !== "student" && (
            <button className="blue" onClick={() => showModalToUploadFunction('feedback')}>Dodaj Feedback</button>
          )}
          <button className="blue" onClick={() => handleViewFiles(id, 'feedback')}>Zobacz Feedback</button>
        </div>
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
    </div>
  );
};


export default HomeworkTask;
