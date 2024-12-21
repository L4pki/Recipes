import React, { useEffect, useState } from "react";
import { Link, NavLink } from "react-router-dom";
import "./Header.css"; // Импортируем стили
import { infoUser } from "../../api/userService";
import Popup from "../AuthPopup/AuthPopup";
import { User } from "../../types/user";

const Header: React.FC = () => {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [username, setUsername] = useState<string | null>(null);
    const [isPopupOpen, setIsPopupOpen] = useState<boolean>(false);

    const fetchUserInfo = async () => {
        try {
            const userInfo = await infoUser();
            setUsername(userInfo.name); // Сохраняем имя пользователя
        } catch (error) {
            console.error(
                "Ошибка при получении информации о пользователе:",
                error
            );
            setUsername(null); // Если произошла ошибка, сбрасываем имя пользователя
        }
    };

    useEffect(() => {
        const checkAuthStatus = () => {
            const token = localStorage.getItem("token");
            setIsAuthenticated(!!token);
            if (token) {
                fetchUserInfo(); // Получаем информацию о пользователе
            } else {
                setUsername(null);
            }
        };
        checkAuthStatus();
        const intervalId = setInterval(checkAuthStatus, 30 * 60 * 1000);

        window.addEventListener("storage", checkAuthStatus);
        return () => {
            clearInterval(intervalId);
            window.removeEventListener("storage", checkAuthStatus);
        };
    }, []);

    const handleLogout = () => {
        localStorage.removeItem("token");
        setIsAuthenticated(false);
        setUsername(null);
    };

    const handleSuccessfulAuth = (userInfo: User) => {
        setIsAuthenticated(true);
        setUsername(userInfo.name); // Устанавливаем имя пользователя
        setIsPopupOpen(false);
        window.location.reload();
    };

    return (
        <header>
            <nav>
                <ul>
                    <h1>Recipes</h1>
                    <div className="header-navigate">
                        <ul>
                            <li>
                                <NavLink
                                    to="/main"
                                    className={({ isActive }) =>
                                        isActive
                                            ? "nav-link nav-link-active"
                                            : "nav-link"
                                    }
                                >
                                    Главная
                                </NavLink>
                            </li>
                            <li>
                                <NavLink
                                    to="/recipes"
                                    className={({ isActive }) =>
                                        isActive
                                            ? "nav-link nav-link-active"
                                            : "nav-link"
                                    }
                                >
                                    Рецепты
                                </NavLink>
                            </li>
                            <li>
                                <NavLink
                                    to="/favorite"
                                    className={({ isActive }) =>
                                        isActive
                                            ? "nav-link nav-link-active"
                                            : "nav-link"
                                    }
                                >
                                    Избранные
                                </NavLink>
                            </li>
                        </ul>
                    </div>

                    {isAuthenticated ? (
                        <>
                            <li>
                                <Link to="/profile">
                                    {username ? username : "Пользователь"}
                                </Link>
                            </li>
                            <li>
                                <button onClick={handleLogout}>Выйти</button>
                            </li>
                        </>
                    ) : (
                        <div>
                            <button onClick={() => setIsPopupOpen(true)}>
                                Войти
                            </button>
                            <Popup
                                isOpen={isPopupOpen}
                                isLogin={"login"} // Передаем текущий режим
                                onClose={() => setIsPopupOpen(false)}
                                onSuccessfulAuth={handleSuccessfulAuth}
                            />
                        </div>
                    )}
                </ul>
            </nav>
        </header>
    );
};

export default Header;
