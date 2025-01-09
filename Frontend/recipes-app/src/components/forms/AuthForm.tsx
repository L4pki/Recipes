import React, { useState } from "react";
import { User } from "../../types/user";
import "./AuthForm.css";

interface AuthFormProps {
    user: User;
    onChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
    onSubmit: (event: React.FormEvent<HTMLFormElement>) => void;
    onChangeMode: (mode: "login" | "regist" | "choise" | "change") => void; 
    isLoginMode: "login" | "regist" | "choise" | "change"; 
    onClose: () => void; 
}

const AuthForm: React.FC<AuthFormProps> = ({
    user,
    onChange,
    onSubmit,
    onChangeMode,
    isLoginMode,
    onClose,
}) => {
    const [confirmPassword, setConfirmPassword] = useState(""); 
    const [passwordError, setPasswordError] = useState(""); 
    const [passwordMismatchError, setPasswordMismatchError] = useState("");

    const handlePasswordChange = (
        event: React.ChangeEvent<HTMLInputElement>
    ) => {
        const { value } = event.target;
        setConfirmPassword(value);

        if (value.length < 8) {
            setPasswordError("Минимум 8 символов");
        } else {
            setPasswordError("");
        }

        if (user.password && value !== user.password) {
            setPasswordMismatchError("Пароли не совпадают");
        } else {
            setPasswordMismatchError("");
        }
    };
    
    const isPasswordsMatch = user.password === confirmPassword;

    return (
        <form onSubmit={onSubmit} className="auth-form">
            {isLoginMode === "choise" && (
                <div className="auth-container">
                    <h2 className="auth-title">Войдите в профиль</h2>
                    <p className="auth-text">
                        Добавлять рецепты могут только зарегистрированные
                        пользователи
                    </p>
                    <div className="auth-form-buttons">
                        <button
                            className="auth-button-login"
                            onClick={() => onChangeMode("login")}
                        >
                            Войти
                        </button>
                        <button
                            className="auth-button-exit"
                            onClick={() => onChangeMode("regist")}
                        >
                            Регистрация
                        </button>
                    </div>
                </div>
            )}
            {isLoginMode === "login" && (
                <>
                    <div className="auth-container">
                        <h2 className="auth-title">Войти</h2>
                        <div className="auth-input-container">
                            <input
                                className="auth-input"
                                type="text"
                                name="login"
                                placeholder="Логин"
                                value={user.login}
                                onChange={onChange}
                                required
                            />
                            <input
                                className="auth-input"
                                type="password"
                                name="password"
                                placeholder="Пароль"
                                value={user.password}
                                onChange={onChange}
                                required
                            />
                        </div>
                        <div className="auth-form-buttons">
                            <button className="auth-button-login">Войти</button>
                            <button
                                className="auth-button-exit"
                                onClick={onClose}
                            >
                                Отмена
                            </button>
                        </div>
                        <div className="auth-form-change">
                            <button
                                className="auth-button-change"
                                onClick={() => onChangeMode("regist")}
                            >
                                У меня еще нет аккаунта
                            </button>
                        </div>
                    </div>
                </>
            )}
            {isLoginMode === "regist" && (
                <>
                    <div className="auth-container">
                        <h2 className="auth-title">Регистрация</h2>
                        <div className="auth-input-container">
                            <input
                                className="auth-input"
                                type="text"
                                name="name"
                                placeholder="Имя"
                                value={user.name}
                                onChange={onChange}
                                required
                            />
                            <input
                                className="auth-input"
                                type="text"
                                name="login"
                                placeholder="Логин"
                                value={user.login}
                                onChange={onChange}
                                required
                            />
                        </div>
                        <div className="auth-input-pass-container">
                            <div className="input-message">
                                <input
                                    className="auth-input"
                                    type="password"
                                    name="password"
                                    placeholder="Пароль"
                                    value={user.password}
                                    onChange={onChange}
                                    required
                                />
                                <div className="error-message">
                                    Минимум 8 символов
                                </div>
                            </div>
                            <div className="input-message">
                                <input
                                    className="auth-input"
                                    type="password"
                                    name="confirmPassword"
                                    placeholder="Повторите пароль"
                                    value={confirmPassword}
                                    onChange={handlePasswordChange}
                                    required
                                />
                                <div className="error-message">
                                    {passwordMismatchError}
                                </div>
                            </div>
                        </div>
                        <div className="auth-form-buttons">
                            <button
                                className="auth-button-login"
                                disabled={
                                    !isPasswordsMatch || passwordError !== ""
                                }
                            >
                                Зарегистрироваться
                            </button>
                            <button
                                className="auth-button-exit"
                                onClick={onClose}
                            >
                                Отмена
                            </button>
                        </div>
                        <div className="auth-form-change">
                            <button
                                className="auth-button-change"
                                onClick={() => onChangeMode("login")}
                            >
                                У меня уже есть аккаунт
                            </button>
                        </div>
                    </div>
                </>
            )}
            {isLoginMode === "change" && (
                <>
                    <div className="auth-container">
                        <h2 className="auth-title">Подтверждение данных</h2>
                        <div className="auth-input-container">
                            <input
                                className="auth-input"
                                type="text"
                                name="login"
                                placeholder="Логин"
                                value={user.login}
                                onChange={onChange}
                                required
                            />
                            <input
                                className="auth-input"
                                type="password"
                                name="password"
                                placeholder="Пароль"
                                value={user.password}
                                onChange={onChange}
                                required
                            />
                        </div>
                        <div className="auth-form-buttons">
                            <button className="auth-button-login">Сохранить изменения</button>
                            <button
                                className="auth-button-exit"
                                onClick={onClose}
                            >
                                Отмена
                            </button>
                        </div>
                    </div>
                </>
            )}
        </form>
    );
};

export default AuthForm;
