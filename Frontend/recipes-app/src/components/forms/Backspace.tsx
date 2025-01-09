import { useNavigate } from "react-router-dom";
import backspace from "../../assets/images/backspace.png";
import './Backspace.css'

const Backspace: React.FC<{}> = () => {
    const navigate = useNavigate();
    return (
        <div>
            <button
                className="backspace-button"
                onClick={() => navigate(-1)}
            >
                <img src={backspace} alt="Назад" />
                <p className="backspace-text">Назад</p>
            </button>
        </div>
    );
};

export default Backspace;
