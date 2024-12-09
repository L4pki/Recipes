import React from 'react';
import { User } from '../../types/user';

interface UserEditFormProps {
  editUser: User | null;
  validationErrors: { login: string; password: string };
  handleChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  handleUpdateUser:  () => Promise<void>;
  setIsEditing: (isEditing: boolean) => void;
  isValid: () => boolean;
}

const UserEditForm: React.FC<UserEditFormProps> = ({
  editUser ,
  validationErrors,
  handleChange,
  handleUpdateUser ,
  setIsEditing,
  isValid,
}) => {
  return (
    <div className="edit-form">
      <h2>Редактировать данные</h2>
      <label>
        Логин:
        <input
          type="text"
          name="login"
          value={editUser?.login}
          readOnly
          className="readonly-input"
        />
      </label>
      <label>
        Пароль:
        <input
          type="password"
          name="password"
          onChange={handleChange}
          className={validationErrors.password ? 'error' : ''}
        />
        {validationErrors.password && <div className="error">{validationErrors.password}</div>}
      </label>
      <label>
        Имя:
        <input
          type="text"
          name="name"
          value={editUser?.name}
          onChange={handleChange}
        />
      </label>
      <label>
        О себе:
        <input
          type="text"
          name="about"
          value={editUser?.about}
          onChange={handleChange}
        />
      </label>
      <button
        onClick={handleUpdateUser }
        className="update-button"
        disabled={!isValid()}
      >
        Сохранить изменения
      </button>
      <button onClick={() => setIsEditing(false)} className="cancel-button">Отменить</button>
    </div>
  );
};

export default UserEditForm;
