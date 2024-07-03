import React from 'react';

const MsgBox = ({ message, onClose }) => {
  if (!message) return null;

  return (
    <div className="msgbox-overlay">
      <div className="msgbox">
        <p>{message}</p>
        <button onClick={onClose}>Ok</button>
      </div>
    </div>
  );
};

export default MsgBox;
