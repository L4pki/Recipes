import axios from "axios";

const handleError = (error: any) => {
    if (axios.isAxiosError(error)) {
        if (error.response) {
            return {
                message: error.response.data.message || "Произошла ошибка.",
                status: error.response.status,
            };
        } else {
            return { message: error.message, status: 500 };
        }
    } else {
        return { message: "Неизвестная ошибка", status: 500 };
    }
};

export default handleError;
