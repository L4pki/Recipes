import React, { useEffect, useState } from "react";
import AuthForm from "../forms/AuthForm";
import "./AuthPopup.css";
import { User } from "../../types/user";
import { infoUser } from "../../api/userService";
import { loginUser, registerUser } from "../../api/authService";
import Close from "../../assets/images/Close.png";

interface PopupProps {
    isOpen: boolean;
    isLogin: "login" | "regist" | "choise" | "change";
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
        "login" | "regist" | "choise" | "change"
    >("choise");
    const [user, setUser] = useState<User>({
        login: "",
        password: "",
        name: "",
        about: "",
    });
    const [errorMessage, setErrorMessage] = useState<string>("");

    useEffect(() => {
        setIsLoginMode(isLogin);
        setErrorMessage("");
    }, [isOpen, isLogin]);

    if (!isOpen) return null;

    const handleAuthenticate = async (
        event: React.FormEvent<HTMLFormElement>
    ) => {
        event.preventDefault();
        setErrorMessage("");
        try {
            let userInfo;
            if (isLoginMode === "login") {
                const loginResult = await loginUser({
                    login: user.login,
                    password: user.password,
                });
                if (loginResult && loginResult.status) {
                    setErrorMessage(loginResult.message);
                    return;
                }
                userInfo = await infoUser();
            } else if (isLoginMode === "change") {
                const loginResult = await loginUser({
                    login: user.login,
                    password: user.password,
                });
                if (loginResult && loginResult.status) {
                    setErrorMessage(loginResult.message);
                    return;
                }
                userInfo = await infoUser();
            } else if (isLoginMode === "regist"){
                const registerResult = await registerUser(user);
                if (registerResult && registerResult.status) {
                    setErrorMessage(registerResult.message);
                    return;
                }
                userInfo = await infoUser();
            }
            setUser(userInfo);
            onClose();
            onSuccessfulAuth(userInfo);
        } catch (error) {
            //console.error("Ошибка аутентификации:", error);
            setErrorMessage(
                "Произошла ошибка при аутентификации. Пожалуйста, попробуйте еще раз."
            );
        }
    };

    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = event.target;
        setUser({
            ...user,
            [name]: value,
        });
    };

    const handleChangeMode = (
        mode: "login" | "regist" | "choise" | "change"
    ) => {
        setIsLoginMode(mode);
        setErrorMessage("");
        if (mode === "regist") {
            setUser({ login: "", password: "", name: "", about: "" });
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
                    user={user}
                    onChange={handleChange}
                    onSubmit={handleAuthenticate}
                    onChangeMode={handleChangeMode}
                    isLoginMode={isLoginMode}
                    onClose={onClose}
                />
            </div>
        </div>
    );
};

export default Popup;
