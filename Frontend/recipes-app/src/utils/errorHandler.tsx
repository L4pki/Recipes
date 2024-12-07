import axios from 'axios';

const handleError = (error: any) => {
    if (axios.isAxiosError(error)) {
        if (error.response) {
            console.error('Ошибка:', error.response.data);
        } else {
            console.error('Ошибка:', error.message);
        }
    } else {
        console.error('Ошибка:', error);
    }
};

export default handleError;