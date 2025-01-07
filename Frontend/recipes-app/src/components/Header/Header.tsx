import React, { useEffect, useState } from "react";
import { Link, NavLink } from "react-router-dom";
import "./Header.css";
import { infoUser } from "../../api/userService";
import Popup from "../AuthPopup/AuthPopup";
import { User } from "../../types/user";
import login from "../../assets/images/login.png";
import exit from "../../assets/images/exit.png";

const Header: React.FC = () => {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [username, setUsername] = useState<string | null>(null);
    const [isPopupOpen, setIsPopupOpen] = useState<boolean>(false);

    const fetchUserInfo = async () => {
        try {
            const userInfo = await infoUser();
            setUsername(userInfo.name);
        } catch (error) {
            console.error(
                "Ошибка при получении информации о пользователе:",
                error
            );
            localStorage.removeItem("token");
            setIsAuthenticated(false);
            setUsername(null);
        }
    };

    useEffect(() => {
        const checkAuthStatus = () => {
            const token = localStorage.getItem("token");
            setIsAuthenticated(!!token);
            if (token) {
                fetchUserInfo();
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
        setUsername(userInfo.name);
        setIsPopupOpen(false);
        window.location.reload();
    };

    return (
        <header>
            <ul className="header-block">
                <div className="header-navigate">
                    <h1 className="header-title">Recipes</h1>
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
                </div>
                <div className="header-user">
                    <div className="header-user-block">
                    <img className="icon" src={login} alt="Иконка пользователя" />
                    {isAuthenticated ? (
                        <>
                        
                            <li>
                                <Link className="header-user-text" to="/profile">
                                    {"Привет, "}{ username ? username : "Пользователь"}
                                </Link>
                            </li>
                            <div className="separator"></div>
                            <li>
                                <img className="icon" src={exit} alt="Иконка выхода" onClick={handleLogout} />
                            </li>
                        </>
                            
                    ) : (
                        <>
                            
                            <button className="header-user-text" onClick={() => setIsPopupOpen(true)}>
                                Войти
                            </button>
                            <Popup
                                isOpen={isPopupOpen}
                                isLogin={"login"}
                                onClose={() => setIsPopupOpen(false)}
                                onSuccessfulAuth={handleSuccessfulAuth}
                            />
                        </>
                    )}
                    </div>
                </div>
            </ul>
        </header>
    );
};

export default Header;
