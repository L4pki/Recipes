import React from 'react';
import { User } from '../../types/user';

interface AuthFormProps {
    user: User;
    onChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
    onSubmit: (event: React.FormEvent<HTMLFormElement>) => void;
    isLoginMode: boolean;
}

const AuthForm: React.FC<AuthFormProps> = ({ user, onChange, onSubmit, isLoginMode }) => {
    return (
        <form onSubmit={onSubmit}>
            {!isLoginMode && (
                <>
                    <input
                        type="text"
                        name="login"
                        placeholder="Логин"
                        value={user.login}
                        onChange={onChange}
                        required
                    />
                    <input
                        type="password"
                        name="password"
                        placeholder="Пароль"
                        value={user.password}
                        onChange={onChange}
                        required
                    />
                    <input
                        type="text"
                        name="name"
                        placeholder="Имя"
                        value={user.name}
                        onChange={onChange}
                        required
                    />
                    <input
                        type="text"
                        name="about"
                        placeholder="О себе"
                        value={user.about}
                        onChange={onChange}
                    />
                </>
            )}
            {isLoginMode && (
                <>
                    <input
                        type="text"
                        name="login"
                        placeholder="Логин"
                        value={user.login}
                        onChange={onChange}
                        required
                    />
                    <input
                        type="password"
                        name="password"
                        placeholder="Пароль"
                        value={user.password}
                        onChange={onChange}
                        required
                    />
                </>
            )}
            <button type="submit">{isLoginMode ? 'Войти' : 'Зарегистрироваться'}</button>
        </form>
    );
};

export default AuthForm;