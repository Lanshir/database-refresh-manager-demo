/** Ожидание N милисекунд. */
const Sleep = async function (ms: number) {
    return new Promise(resolve => setTimeout(resolve, ms));
};

export default Sleep;
