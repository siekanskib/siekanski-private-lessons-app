import React, { useState, useEffect } from 'react';
import './FilesModal.css';

const FilesModal = ({ isOpen, filesUrls, onClose }) => {
    if (!isOpen) return null;
  
    return (
      <div className="modal-overlay">
        <div className="modal-content">
          <h2>Pliki</h2>
          <ul>
            {filesUrls.map((url, index) => (
              <li key={index}>
                <a href={url} target="_blank" rel="noopener noreferrer">Plik {index + 1}</a>
              </li>
            ))}
          </ul>
          <button onClick={onClose}>Zamknij</button>
        </div>
      </div>
    );
  };

  export default FilesModal;