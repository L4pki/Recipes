import React, { useEffect, useState } from "react";
import AuthForm from "../forms/AuthForm";
import "./AuthPopup.css";
import { User } from "../../types/user";
import { infoUser } from "../../api/userService";
import { loginUser, registerUser } from "../../api/authService";
import Close from "../../assets/images/Close.png";

interface PopupProps {
    isOpen: boolean;
    isLogin: "login" | "regist" | "choise"; // Передаем режим
    onClose: () => void;
    onSuccessfulAuth: (userInfo: User) => void;
}

const Popup: React.FC<PopupProps> = ({
    isOpen,
    isLogin,
    onClose,
    onSuccessfulAuth,
}) => {
    const [isLoginMode, setIsLoginMode] = useState<
        "login" | "regist" | "choise"
    >("choise");
    const [user, setUser] = useState<User>({
        login: "",
        password: "",
        name: "",
        about: "",
    });
    const [errorMessage, setErrorMessage] = useState<string>(""); // Состояние для хранения сообщения об ошибке

    useEffect(() => {
        setIsLoginMode(isLogin);
        setErrorMessage(""); // Сбросить сообщение об ошибке при открытии попапа
    }, [isOpen, isLogin]);

    if (!isOpen) return null;

    const handleAuthenticate = async (
        event: React.FormEvent<HTMLFormElement>
    ) => {
        event.preventDefault();
        setErrorMessage(""); // Сбросить сообщение об ошибке перед новой попыткой аутентификации
        try {
            let userInfo;
            if (isLoginMode === "login") {
                const loginResult = await loginUser({
                    login: user.login,
                    password: user.password,
                });
                if (loginResult && loginResult.status) {
                    setErrorMessage(loginResult.message); // Устанавливаем сообщение об ошибке
                    return; // Выходим из функции, если произошла ошибка
                }
                userInfo = await infoUser();
            } else {
                const registerResult = await registerUser(user);
                if (registerResult && registerResult.status) {
                    setErrorMessage(registerResult.message); // Устанавливаем сообщение об ошибке
                    return; // Выходим из функции, если произошла ошибка
                }
                userInfo = await infoUser();
            }
            setUser(userInfo);
            onClose(); // Закрываем попап после аутентификации
            onSuccessfulAuth(userInfo); // Обновляем состояние в Header
        } catch (error) {
            console.error("Ошибка аутентификации:", error);
            setErrorMessage(
                "Произошла ошибка при аутентификации. Пожалуйста, попробуйте еще раз."
            ); // Общая ошибка
        }
    };

    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = event.target;
        setUser({
            ...user,
            [name]: value,
        });
    };

    const handleChangeMode = (mode: "login" | "regist" | "choise") => {
        setIsLoginMode(mode);
        setErrorMessage("");
        if (mode === "regist") {
            setUser({ login: "", password: "", name: "", about: "" }); // Сбросить поля при переключении на регистрацию
        }
    };

    return (
        <div className="popup-overlay">
            <div className={`popup-content ${isLoginMode}`}>
                <button className="popup-close" onClick={onClose}>
                    <img src={Close} alt={"Закрыть"} />
                </button>
                <div className="error-message-box">
                    {errorMessage && (
                        <div className="error-message">{errorMessage}</div>
                    )}
                </div>

                <AuthForm
                    user={user} // Передаем пользователя
                    onChange={handleChange} // Обработчик изменения
                    onSubmit={handleAuthenticate}
                    onChangeMode={handleChangeMode} // Передаем onChangeMode
                    isLoginMode={isLoginMode} // Передаем режим
                    onClose={onClose}
                />
            </div>
        </div>
    );
};

export default Popup;
