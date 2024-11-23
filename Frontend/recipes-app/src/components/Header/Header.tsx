import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import './Header.css'; // Импортируем стили

const Header: React.FC = () => {
    const [isAuthenticated, setIsAuthenticated] = useState(false);

    const checkAuthStatus = () => {
        const token = localStorage.getItem("token");
        setIsAuthenticated(!!token); 
    };

    useEffect(() => {
        checkAuthStatus();
        window.addEventListener("storage", checkAuthStatus);
        return () => {
            window.removeEventListener("storage", checkAuthStatus);
        };
    }, []);

    const handleLogout = () => {
        localStorage.removeItem("token");
        setIsAuthenticated(false);
    };

    return (
        <header>
            <nav>
                <ul>
                    <h1>Recipes</h1>
                    <li>
                        <Link to="/main">Главная</Link>
                    </li>
                    <li>
                        <Link to="/recipes">Рецепты</Link>
                    </li>
                    <li>
                        <Link to="/favorite">Избранные</Link>
                    </li>
                    {isAuthenticated ? (
                        <>
                            <li>
                                <Link to="/profile">Профиль</Link>
                            </li>
                            <li>
                                <button onClick={handleLogout}>Выйти</button>
                            </li>
                        </>
                    ) : (
                        <li>
                            <Link to="/authorization">Войти</Link>
                        </li>
                    )}
                </ul>
            </nav>
        </header>
    );
};

export default Header;
