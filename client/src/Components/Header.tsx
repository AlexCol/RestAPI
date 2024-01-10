import React, { ReactNode } from 'react';
import styles from './Header.module.css'

/* //forma 1
interface Props {
    title: string
}

export default function Header({title}: Props) {
    return (
        <header className={styles.cabecalho}>
            <h1>{title}</h1>
        </header>
    );
}
*/


/* //forma 2
export default function Header({children}: any) {
    return (
        <header className={styles.cabecalho}>
            <h1>{children}</h1>
        </header>
    );
}
*/

interface Props {
    counter: number
}
export default function Header({counter}: Props) {
    return (
        <header className={styles.cabecalho}>
            <h1>Counter: {counter}</h1>
        </header>
    );
}