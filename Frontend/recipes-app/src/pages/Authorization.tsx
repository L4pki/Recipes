import React, { useState } from "react";
import { User } from "../types/user";
import { registerUser , loginUser  } from "../api/authService";
import { infoUser } from "../api/userService";
import { useNavigate } from "react-router-dom";

const Authorization: React.FC = () => {
    const [user, setUser ] = useState<User>({
        login: "",
        password: "",
        name: "",
        about: "",
    });

    const history = useNavigate();
    const [isLoginMode, setIsLoginMode] = useState<boolean>(true);
    const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);

    const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        if (isLoginMode) {
            await loginUser ({ login: user.login, password: user.password });
            const userInfo = await infoUser ();
            setUser (userInfo);
            console.log(userInfo);
            setIsAuthenticated(true);
        } else {
            await registerUser (user);
            const userInfo = await infoUser ();
            setUser (userInfo);
            setIsAuthenticated(true);
        }
        history("/main");
        window.location.reload();
    };

    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = event.target;
        setUser ({
            ...user,
            [name]: value,
        });
    };

    const toggleMode = () => {
        setIsLoginMode(!isLoginMode);
        if (isLoginMode) {
            setUser ({
                login: "",
                password: "",
                name: "",
                about: "",
            });
        }
    };

    return (
        <div className="auth-container">
            {!isAuthenticated ? (
                <div className="auth-form-container">
                    <h2 className="auth-title">{isLoginMode ? "Вход" : "Регистрация"}</h2>
                    
                    <button className="toggle-mode-button" onClick={toggleMode}>
                        Перейти к {isLoginMode ? "регистрации" : "входу"}
                    </button>
                </div>
            ) : (
                <div className="user-info">
                    <h2 className="user-greeting">Добро пожаловать, {user.name}!</h2>
                </div>
            )}
        </div>
    );
};

export default Authorization;