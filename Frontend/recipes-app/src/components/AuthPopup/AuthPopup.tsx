import React from "react";
import './AuthPopup.css'; // Добавьте стили для вашего попапа

interface PopupProps {
    isOpen: boolean;
    isLogin: boolean | null;
    onClose: () => void;
    onToggleMode: () => void;
}

const Popup: React.FC<PopupProps> = ({ isOpen, isLogin, onClose, onToggleMode }) => {
    if (!isOpen) return null;
    if (isLogin == null){
        return (
            <div className="popup-overlay">
            <div className="popup-content">
                
                <button onClick={onClose}>Закрыть</button>
            </div>
        </div>
        );
    }

    return (
        <div className="popup-overlay">
            <div className="popup-content">
                <h2>{isLogin ? "Вход" : "Регистрация"}</h2>
                
                {isLogin ? (
                    <div>
                        <p>Введите свои учетные данные для входа.</p>
                        {/* Здесь можно добавить вашу форму входа */}
                        <button onClick={onClose}>Войти</button>
                    </div>
                ) : (
                    <div>
                        <p>Введите свои данные для регистрации.</p>
                        {/* Здесь можно добавить вашу форму регистрации */}
                        <button onClick={onClose}>Зарегистрироваться</button>
                    </div>
                )}

                <p>
                    {isLogin ? "Нет аккаунта?" : "Уже есть аккаунт?"}
                    <button onClick={onToggleMode}>
                        {isLogin ? "Зарегистрироваться" : "Войти"}
                    </button>
                </p>
                <button onClick={onClose}>Закрыть</button>
            </div>
        </div>
    );
};

export default Popup;
