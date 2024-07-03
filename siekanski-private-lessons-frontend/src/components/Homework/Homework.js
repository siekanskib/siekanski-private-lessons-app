import React, { useState, useEffect } from 'react';
import './Homework.css';
import HomeworkTask from './HomeworkTask';
import axios from 'axios';
import { useAuth } from '../../context/AuthContext';

const Homework = ({ isOpen, homeworkId, onClose }) => {
    const [homeworkDetails, setHomeworkDetails] = useState(null);
    const [tasks, setTasks] = useState([]);
    const [newTaskName, setNewTaskName] = useState('');
    const [newTaskContent, setNewTaskContent] = useState('');
    const [newTaskFeedback, setNewTaskFeedback] = useState('');

    const { currentUser } = useAuth();

    const fetchHomeworkDetails = async () => {
        if (!homeworkId) return;

        try {
            const response = await axios.get(`/api/homeworks/${homeworkId}`, {
                headers: { Authorization: `Bearer ${localStorage.getItem('token')}` },
            });
            setHomeworkDetails(response.data);
            const tasksResponse = await axios.get(`/api/homeworktasks/${homeworkId}/tasks`, {
                headers: { Authorization: `Bearer ${localStorage.getItem('token')}` },
            });
            setTasks(tasksResponse.data.$values);
        } catch (error) {
            console.error('Błąd podczas ładowania pracy domowej', error);
        }
    };
    useEffect(() => {
        if (isOpen) {
            setTasks([]);
            fetchHomeworkDetails();
        }
    }, [isOpen, homeworkId]);

    if (!isOpen) return null;


    const addTask = async () => {
        try {
            await axios.post(`/api/homeworktasks/add`, {
                name: newTaskName,
                homeworkId: homeworkId,
                content: newTaskContent,
                feedback: newTaskFeedback
            }, {
                headers: { Authorization: `Bearer ${localStorage.getItem('token')}` },
            });
            const tasksResponse = await axios.get(`/api/homeworktasks/${homeworkId}/tasks`, {
                headers: { Authorization: `Bearer ${localStorage.getItem('token')}` },
            });
            setTasks(tasksResponse.data.$values);
            setNewTaskName('');
            setNewTaskContent('');
            setNewTaskFeedback('');
        } catch (error) {
            console.error('Błąd podczas dodawania zadania', error);
        }
    };

    const handleChangeStatus = async (newStatus) => {
        try {
            await axios.put(`/api/homeworks/${homeworkId}/status`, { status: newStatus }, {
                headers: { Authorization: `Bearer ${localStorage.getItem('token')}` },
            });
            setHomeworkDetails(prevDetails => ({ ...prevDetails, status: newStatus }));

        } catch (error) {
            console.error('Błąd podczas aktualizacji statusu pracy domowej', error);
        }
    };

    return (
        <div className="modal-overlay">
            <div className="modal-content">
                <button className="close-modal" onClick={onClose}>X</button>
                <h2 className="homework-description">{homeworkDetails?.description}</h2>
                <form className="status-form">
                    <h3>Status pracy domowej:</h3>
                    <div className="radio-group">
                        <div>
                            <input
                                type="radio"
                                id="brak-pracy-domowej"
                                name="status"
                                value="Brak pracy domowej"
                                checked={homeworkDetails?.status === "Brak pracy domowej"}
                                onChange={() => handleChangeStatus("Brak pracy domowej")}
                                disabled={currentUser.role === "student"}
                            />
                            <label htmlFor="do-zrobienia">Brak pracy domowej</label>
                        </div>
                        <div>
                            <input
                                type="radio"
                                id="do-zrobienia"
                                name="status"
                                value="Do zrobienia"
                                checked={homeworkDetails?.status === "Do zrobienia"}
                                onChange={() => handleChangeStatus("Do zrobienia")}
                            />
                            <label htmlFor="do-zrobienia">Do zrobienia</label>
                        </div>
                        <div>
                            <input
                                type="radio"
                                id="do-sprawdzenia"
                                name="status"
                                value="Do sprawdzenia"
                                checked={homeworkDetails?.status === "Do sprawdzenia"}
                                onChange={() => handleChangeStatus("Do sprawdzenia")}
                            />
                            <label htmlFor="do-sprawdzenia">Do sprawdzenia</label>
                        </div>
                        <div>
                            <input
                                type="radio"
                                id="sprawdzona"
                                name="status"
                                value="Sprawdzona"
                                checked={homeworkDetails?.status === "Sprawdzona"}
                                onChange={() => handleChangeStatus("Sprawdzona")}
                                disabled={currentUser.role === "student"}
                            />
                            <label htmlFor="sprawdzona">Sprawdzona</label>
                        </div>
                    </div>
                </form>
                {currentUser.role !== "student" && (
                    <div className="new-task-form">
                        <input
                            type="text"
                            placeholder="Nazwa zadania"
                            value={newTaskName}
                            onChange={(e) => setNewTaskName(e.target.value)}
                        />
                        <textarea
                            placeholder="Treść zadania"
                            value={newTaskContent}
                            onChange={(e) => setNewTaskContent(e.target.value)}
                        ></textarea>
                        <textarea
                            placeholder="Feedback"
                            value={newTaskFeedback}
                            onChange={(e) => setNewTaskFeedback(e.target.value)}
                        ></textarea>
                        <button onClick={addTask}>Dodaj zadanie</button>
                    </div>
                )}
                <div className="tasks-container">
                    {console.log(tasks)}
                    {tasks.map((task, index) => (
                        <HomeworkTask
                            key={index}
                            index={index}
                            name={task.name}
                            id={task.id}
                            homeworkId={task.homeworkId}
                            contentProp={task.content}
                            feedbackProp={task.feedback}
                            refreshTasks={fetchHomeworkDetails}
                        />
                    ))}
                </div>
            </div>
        </div>
    );
};

export default Homework;
