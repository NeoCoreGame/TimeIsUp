# TIME IS UP
## 1. Historial de versiones
### GDD versión 0.1.0:
   - Se ha añadido pegi al apartado de introducción
   - Se ha completado el apartado de monetización y modelo de negocio
   - Se ha separado los apartados de personajes y enemigos de la narrativa
   - Se han completado los apartados de jugabilidad, personajes jugables, enemigos y controles
   - Se ha creado el apartado de interfaces y pantalla, diseño de escenario y música y sonido
   - Se han añadido los diseños de personajes a los apartados de arte
   - Se han revisado los apartados existentes

## 2. Introducción
El presente documento introduce el Game Design Document (GDD) del videojuego “Time Is Up”. A lo largo del desarrollo de este, se explicará a detalle las características que lo definen, así como el propósito del juego y su modelo de negocio. 

### Concepto general
“Time Is Up” consiste en un videojuego competitivo 3D, basado en la temática de “Arena”, en la que se concentran dos o más jugadores que competirán entre sí para ganar tiempo de partida. 
Las partidas consistirán alrededor de 15-20 minutos en la que los jugadores tendrán un tiempo base (5 minutos) y necesitarán aumentarlo para que la partida no acabe. Para ganar tiempo tendrán que matar oleadas de enemigos y hacerse más fuertes para así poder derrotar al contrincante y que este se quede sin tiempo.

### Público general
Al ser un juego competitivo, nuestro objetivo es hacer llegar nuestro videojuego a un público mayor de 16 años, haciendo énfasis en el público más joven.
Se busca que el usuario haga uso de sus habilidades y conocimientos como jugador para intentar llegar a ser el mejor. Aquellos jugadores que hayan tenido una experiencia previa con shooters o juegos de acción, tendrán una ventaja frente a los jugadores noveles.

### Propósitos del juego
El objetivo principal del juego competitivo es proporcionar una experiencia emocionante y desafiante para los jugadores, donde se pongan a prueba y desarrollen tanto sus habilidades estratégicas como su destreza física y mental en un entorno de competencia directa. Los jugadores deberán mejorar constantemente para superar a sus oponentes, adaptándose a cambios en las mecánicas del juego y optimizando su rendimiento.

### Plataformas
Time Is Up estará disponible para WebGL, por lo que será posible jugarlo a través de ordenador con teclado y ratón, y desde móvil con controles táctiles. 

### Sinópsis
En un universo al borde de la destrucción, una fuerza desconocida ha convocado a los mejores guerreros de distintas realidades a Fricb, una arena mística donde el tiempo es la única moneda de supervivencia. Estos héroes no llegaron por voluntad propia; han sido atraídos por una entidad enigmática que controla el destino del cosmos. Su misión es clara: luchar y ganar tiempo, o ver cómo su mundo se desvanece en el olvido. En cada combate, dos equipos se enfrentan frenéticamente contra oleadas de enemigos y entre sí, para acumular tiempo vital que podría ser la única salvación de sus respectivos universos. Solo un equipo logrará alargar la vida de su mundo, mientras el otro verá cómo el reloj marca el fin de su realidad. ¿Tienes lo necesario para derrotar a tus enemigos y salvar tu universo antes de que Time Is Up?

### Género
El videojuego "Time Is Up" es del género arena competitiva en 3D. Está diseñado para partidas cortas y frenéticas en las que los jugadores deben competir entre sí, enfrentándose a oleadas de enemigos para ganar tiempo de juego. Combina elementos de shooters en primera persona y gestión de recursos, donde el tiempo es el recurso clave.

### Estilo visual
El estilo del proyecto se desarrollará en 3D, tanto el escenario como los personajes, con texturas cartoon para que sea más cercano al jugador. Las partidas serán en primera persona, para que el juego sea más inmersivo.

### PEGI
El juego posee un PEGI 7, puesto que se puede observar violencia implícita al tener que eliminar a tus enemigos, pero no llega a ser muy gráfica.

## 3. Monetización y modelo de negocio
Se ha optado por un juego **Free To Play** para que se pueda llegar a la mayor cantidad de público posible y que todos puedan disfrutar sin compromiso.

Se ha optado por un modelo de negocio Freemium, con monetización a través de microtransacciones. De esta forma, cualquiera que quiera acceder al juego podrá realizarlo sin problemas y la empresa se mantendrá a flote a través de pagos opcionales dentro del juego.
### Mapa de empatía
1. ¿Qué piensa y siente el jugador?
   
   Los jugadores quieren destacar frente a sus oponentes y ser reconocidos por sus habilidades y estilo de juego único. Disfrutan la adrenalina de competir bajo presión y personalizar sus avatares para reflejar sus logros. Se frustran si no pueden dedicar el tiempo suficiente para mejorar y les molesta cometer errores que les hacen perder tiempo en las partidas.
2. ¿Qué ve?
   
   El juego ofrece gráficos cartoon vibrantes y enemigos visualmente distintos que facilitan identificar sus habilidades y recompensas. En el entorno externo, los jugadores ven a otros compartiendo sus skins y logros en redes sociales, mientras que influencers muestran estrategias para maximizar el uso del tiempo y progresar.
3. ¿Qué dice y hace?

   Dentro del juego, participan en chats y forman equipos para discutir tácticas y celebrar o lamentar sus resultados. Fuera del juego, comentan en foros, comparten clips de sus jugadas y participan en eventos organizados por la comunidad, demostrando su compromiso.
4. ¿Qué oye?

    Reciben recomendaciones de amigos y otros jugadores sobre qué mejorar y qué comprar en la tienda. También siguen a influencers que comparten estrategias y discuten nuevas actualizaciones y mecánicas, lo que les ayuda a adaptarse a los cambios del juego.
5. ¿Qué esfuerzos realiza?

    Los jugadores invierten tiempo en dominar estrategias y aprender nuevas mecánicas, preocupándose por no poder dedicarle suficiente tiempo para progresar al mismo ritmo que otros. Temen perder tiempo en errores pequeños y buscan optimizar cada segundo en las partidas.
6. ¿Qué resultados espera?

    Aspiran a ser los últimos en pie, acumulando colecciones completas de skins que reflejen su progreso. Buscan reconocimiento en la comunidad, participar en torneos y compartir sus logros para demostrar su dominio del juego y obtener notoriedad.
### Caja de herramientas
El juego es gratuito con un modelo **freemium** que monetiza mediante microtransacciones estéticas (skins y efectos visuales) y un **Battle Pass**. Se dirige a jugadores competitivos de entre 16 y 30 años que valoran la personalización y disfrutan de partidas rápidas. 
La propuesta de valor se centra en ofrecer una experiencia competitiva con opciones visuales únicas sin alterar la jugabilidad.

1. Flujo de Ingresos: los ingresos provienen de **ventas de microtransacciones**, **Battle Passes** y **eventos especiales** con contenido limitado.
2. Oferta de Valor:
    - **Juego competitivo gratuito** con mecánicas basadas en la gestión del tiempo.
    - **Personalización estética** (skins, efectos visuales, emotes) para destacar visualmente sin ventajas en el juego.
    - **Eventos y torneos** que recompensan a los jugadores con contenido exclusivo.

3. Relaciones:
    - **Recibimos**: Ingresos a través de compras opcionales y visibilidad mediante el contenido compartido por la comunidad.
    - **Ofrecemos**: Una experiencia competitiva gratuita con opciones estéticas y contenido adicional a través de eventos y desafíos.
      
4. Red de Valor:
    - **Jugadores principales**: Compran contenido y promueven la comunidad.
    - **Jugadores casuales**: Participan esporádicamente y realizan compras menores.
    - **Influencers**: Generan contenido, promocionan eventos y atraen nuevos jugadores.
    - **Plataformas de distribución**: Steam y Epic Games como canales de venta y visibilidad.
5. Valor Generado:
    - **Lo que ofrecemos**: Juego gratuito, personalización y recompensas limitadas.
    - **Lo que recibimos**: Ingresos, compromiso de la comunidad y promoción indirecta.
  
### Monetización
El modelo de monetización para este juego competitivo basado en arenas y con una estética cartoon se centrará en aprovechar un sistema **freemium** con microtransacciones de contenido puramente estético. 
El objetivo es que el juego siga siendo gratuito y accesible, mientras que las compras dentro del juego sean atractivas, variadas y opcionales, ofreciendo valor añadido a los jugadores sin influir en la jugabilidad. A continuación, se detalla cómo se podría estructurar el sistema de monetización:
1. **Estrategia general de monetización**
   
    La monetización del juego se basará en una combinación de los siguientes métodos:

   1. **Microtransacciones de Cosméticos**: El principal método de monetización. Los jugadores podrán comprar skins, efectos visuales para habilidades, diseños de armas y otros elementos que no influyen en el rendimiento del juego.
   2. Battle Pass (Pase de Temporada): Un pase premium con recompensas exclusivas. Los jugadores pueden desbloquear contenido adicional al completar desafíos y objetivos, manteniendo un sentido de progresión.
   3. Eventos Temáticos y Contenido de Tiempo Limitado: Skins, efectos y personalizaciones especiales disponibles solo durante eventos o temporadas específicas.
   4. Monetización de Comunidad: Integración de ítems diseñados por la comunidad o skins colaborativas con creadores de contenido, donde un porcentaje de las ganancias va a los creadores.

2. **Tipos de microtransacciones**
   
    Las microtransacciones se dividirán en varias categorías para ofrecer opciones y variedad a los jugadores:

    1. **Skins de Personaje**
        - Se lanzarán skins basadas en temáticas (fantasía, futurista, cyberpunk, etc.).
        - Skins de nivel básico: Skins visualmente atractivas, pero con un precio reducido. Perfectas para jugadores casuales que buscan personalizar su personaje sin gastar demasiado.
        - Skins de nivel épico o legendario: Skins con efectos visuales avanzados (partículas, animaciones personalizadas) que tendrán un precio superior.
    2. **Efectos Visuales para Habilidades**
        - Los jugadores podrán comprar efectos únicos que cambian la apariencia de habilidades específicas (por ejemplo, un aura de tiempo distorsionado, explosiones de colores, etc.).
        - Algunos efectos solo estarán disponibles durante eventos especiales para crear un sentido de urgencia y exclusividad (explosiones de nieve en navidad, calaveras en halloween, etc).
    3. **Apariencia de Aliados (Enemigos Tanques Aliados)**
        - Los enemigos tanque, una vez convertidos en aliados, podrán tener skins especiales.
        - Los jugadores pueden comprar apariencias únicas para sus aliados para que combinen con sus propias skins.
    4. **Chromas para skin base**
        - Personalización visual de las armas principales y secundarias.
        - Incluye patrones, texturas exclusivas y efectos visuales.
    5. **Animaciones y Emotes**
        - Emotes personalizables que los jugadores pueden usar durante las partidas.
        - Animaciones de victoria y derrota personalizadas.
    6. **Paquetes de Cosméticos**
        - Ofrecer paquetes que incluyan varias skins y efectos a un precio reducido.
        - Paquetes de temporada con temáticas específicas (Halloween, Navidad, Verano, etc.).

3. **Battle Pass (Pase de Temporada)**
   
    El Pase de Temporada es una herramienta diseñada para mantener a los jugadores comprometidos durante un periodo específico,
ofreciendo recompensas exclusivas al completar desafíos y alcanzar ciertos hitos dentro del juego. Este sistema de monetización fomenta la progresión y recompensas por el tiempo invertido,
brindando un motivo para regresar y jugar regularmente.

    1. **Formato del pase**:
        - El pase tiene una duración de aproximadamente 2 meses, dividido en 10 niveles de recompensas.
        - Los jugadores pueden desbloquear dos líneas de recompensas: **Línea Gratuita y Línea Premium**.
            - **Línea Gratuita**: Ofrece recompensas como pequeñas cantidades de monedas del juego, chromas básicos y efectos visuales sencillos.
            - **Línea Premium**: Disponible solo para jugadores que compren el pase, incluye chromas/skins de nivel épico o legendario, emotes exclusivos, efectos visuales avanzados y otros elementos de personalización exclusivos.
    2. **Progresión en el Pase**:
        - Los jugadores obtienen experiencia para el pase al completar desafíos semanales y diarios:
            - **Desafíos diarios**: Ejemplos como "Mata 50 enemigos base" o "Defiende a tu aliado tanque durante 2 oleadas".
            - **Desafíos semanales**: Retos más grandes que requieren mayor dedicación, como "Acumula 30 minutos robados de tu oponente en una semana".
        - La experiencia acumulada desbloquea niveles dentro del pase, con recompensas que se vuelven más valiosas en niveles superiores.
    3. **Monetización del Pase**:
        - **Pase Premium**: Ofrecido a un precio accesible (por ejemplo, 9.99€) para cada temporada.
    4. **Recompensas Exclusivas**:
        - Los elementos del Pase Premium son exclusivos y no estarán disponibles en la tienda al final de la temporada, creando un sentido de urgencia y exclusividad.

4. **Sistema de moneda virtual**

    El juego utilizará una **moneda virtual** que se puede ganar en pequeñas cantidades a través de la jugabilidad y también adquirir mediante compras directas con dinero real.
   
    1. **Monedas de Tiempo**:
        - La moneda virtual se llama Chronos y será representada con un reloj de arena.
        - Los jugadores ganan pequeñas cantidades de Chronos completando ciertos desafíos, como misiones diarias, logros o alcanzando nuevas oleadas.
    2. **Compras con Chronos**:
        - Los Chronos se pueden usar para comprar cualquier skin, chroma, emote o efecto visual en la tienda.
        - También pueden usarse para adquirir el Pase de Temporada y desbloquear contenido adicional.
    3. **Paquetes de Monedas**:
        - Los jugadores pueden comprar Chronos con dinero real. Los precios varían según el paquete:
            - 100 Chronos – $4.99
            - 500 Chronos – $19.99 (10% adicional)
            - 1000 Chronos – $39.99 (20% adicional)
         
5. **Implementación de Marketing In-Game**

    Para asegurar que los jugadores estén al tanto de todas las opciones de monetización, el juego debe tener un sistema de marketing integrado que no sea intrusivo:
   
    1. **Tienda dentro del juego**:
        - Una tienda con secciones bien organizadas que incluye categorías como “Novedades”, “Skins de Temporada” y “Ofertas Limitadas”.
        - Uso de banners para destacar skins populares o eventos actuales.
    2. **Ofertas Especiales y Paquetes**:
        - Paquetes con descuentos por tiempo limitado (por ejemplo, "Paquete de Guerrero del Tiempo: Incluye skin de personaje + efecto de habilidad + emote especial").
        - Ofrecer promociones como "Compra 1000 Chronos y recibe un emote gratis".
    3. **Tienda Rotativa**:
        - Una tienda que cambia las skins y los cosméticos disponibles cada semana, para incentivar a los jugadores a visitar la tienda con regularidad.
### Modelo de Canvas
El **Business Model Canvas** es una herramienta que permite visualizar de manera integral el modelo de negocio de un producto o servicio. En este caso, se desglosa cada uno de los componentes clave del modelo de negocio para el juego.

# METER IMAGEN DEL BUSSINESS MODEL CANVAS

1. **Propuesta de valor**

    El juego es gratuito con un enfoque innovador basado en la gestión del tiempo como recurso. Los jugadores compiten eliminando enemigos y robando tiempo a sus rivales. 
Además, se ofrece personalización estética con skins y efectos visuales que permiten destacar visualmente sin alterar la jugabilidad. Las mecánicas únicas de enemigos aliados y progresión del personaje añaden profundidad y mantienen el interés de los jugadores.

2. **Segmentos de clientes**

    El juego está dirigido a jugadores competitivos, principalmente jóvenes y adultos, a partir de los 16 años, que buscan acción y competencia en tiempo real. 
También se enfoca en aquellos que valoran la personalización y desean reflejar su estilo a través de la estética de los personajes.

3. **Canales**

    El juego se distribuirá digitalmente a través de plataformas como **Steam** y **Epic Games Store**. Las redes sociales (Instagram, Twitter, Facebook y TikTok) se usarán para promocionarlo y mantener la comunicación con la comunidad. Se publicará contenido en **YouTube** y **Twitch**, 
mientras que **Discord** se empleará para eventos y retroalimentación directa con los jugadores. Además, el marketing se reforzará mediante el **boca a boca (Word of mouth)**, que dependerá de la experiencia y recomendaciones de los propios jugadores.

4. **Relación con el Cliente**

    La relación se mantendrá activa mediante soporte a la comunidad, eventos regulares y actualizaciones constantes. Las redes sociales facilitarán la comunicación y permitirán a los jugadores compartir sus experiencias. También se organizarán torneos que ofrezcan recompensas exclusivas y fortalezcan la interacción con los jugadores.

5. **Fuentes de Ingresos**

    El juego generará ingresos a través de **microtransacciones** (venta de skins y efectos visuales), **Battle Pass** con recompensas exclusivas, y contenido limitado durante eventos especiales. También se venderán paquetes de monedas virtuales para adquirir ítems estéticos.

6. **Recursos Clave**

    El equipo de desarrollo compuesto por programadores, diseñadores y artistas es esencial para crear y mantener el juego. La infraestructura tecnológica, incluyendo servidores y software de desarrollo, es crucial para el funcionamiento y evolución del producto.

7. **Actividades Clave**

    Las principales actividades incluyen el desarrollo de contenido, marketing digital y promoción en redes. También se gestionará la comunidad para mantener el vínculo con los jugadores y 
se analizarán métricas de comportamiento y feedback para ajustar la estrategia. La venta de contenido digital (cosméticos) es otra actividad clave para monetizar el juego.

8. **Socios Clave**

    Los socios principales son las **plataformas de distribución** (Steam y Epic Games Store), **influencers** y **creadores de contenido** que ayudan a promocionar el juego. Además, artistas y diseñadores externos contribuyen con contenido estético exclusivo y proveedores de infraestructura aseguran un rendimiento estable del juego.

9. **Estructura de Costes**

    Los costos principales se dividen en desarrollo y mantenimiento del juego, marketing y publicidad, infraestructura tecnológica (servidores), y la inversión en recursos humanos (diseñadores, artistas, programadores y soporte a la comunidad). Estos gastos aseguran la calidad y la promoción continua del producto.

### Marketing

El plan de marketing para este juego se centrará en generar notoriedad, atraer a la audiencia adecuada y mantener a los jugadores comprometidos a largo plazo. El enfoque principal estará en maximizar la visibilidad a través de estrategias de marketing digital, 
colaboraciones con influencers y contenido generado por la comunidad. A continuación, se detallan los principales elementos del plan de marketing, segmentados en fases según el ciclo de vida del juego.

1. Objetivos de Marketing

    El plan de marketing busca atraer a jugadores a partir de 16 años que disfrutan de juegos multijugador competitivos y valoran la personalización estética. Además, se quiere construir una comunidad activa, 
mantener la retención a través de contenido constante y aumentar el valor de vida del cliente (LTV) asegurando que los jugadores se sientan motivados a gastar de manera opcional y justa.

2. Estrategias de Marketing a 2 años

    El marketing se organizará en tres fases principales: Pre-Lanzamiento, Lanzamiento y Post-Lanzamiento.
    
    > Fase 1: Pre-Lanzamiento
    
    El objetivo de esta fase es crear expectativa y curiosidad alrededor del juego antes de su lanzamiento.
    
    1. Construcción de Marca y Expectativa
        - Crear perfiles oficiales del juego en redes sociales: Twitter, Instagram, Facebook, y TikTok.
        - Definir una identidad visual consistente y atractiva, alineada con el estilo visual cartoon del juego.
        - Publicar contenido de forma progresiva:
            - Arte conceptual y vídeos cortos que muestren las mecánicas de tiempo y combate.
            - Teasers de personajes y enemigos: Mostrar a los personajes y tipos de enemigos con descripciones breves para crear intriga.
            - Presentación de la primera arena del juego con imágenes y pequeñas animaciones.
    2. Creación de Comunidad
        - Lanzar un servidor de Discord para atraer a los primeros seguidores y fans.
        - Organizar eventos pequeños (por ejemplo, concursos de arte relacionados con el juego) para incentivar la participación.
    3. Colaboraciones con Influencers
        - Identificar influencers de juegos multijugador y streamers especializados en shooters o juegos competitivos.
        - Enviarles acceso anticipado al juego para pruebas cerradas y permitirles compartir sus experiencias (con restricciones de contenido para mantener la expectación).
        - Invitaciones a beta cerrada: Otorgar acceso limitado a la beta para jugadores que se registren a través de redes sociales.
    4. Campaña de Inscripción a Beta
        - Crear una página web oficial con un contador de tiempo para el lanzamiento y un registro para la beta cerrada.
    5. Campaña de Email Marketing
        - Enviar actualizaciones a los jugadores inscritos en la beta con noticias y detalles exclusivos.
        - Compartir información detrás de cámaras y arte conceptual.
      
    > Fase 2: Lanzamiento
    
    El objetivo de esta fase es asegurar un lanzamiento fuerte, maximizar la visibilidad y atraer a tantos jugadores como sea posible.
    
    1. **Campaña de Lanzamiento en Redes Sociales**
        - Publicar un **tráiler de lanzamiento** que muestre las mecánicas de juego, la progresión de tiempo y las skins disponibles.
        - Utilizar **hashtags virales**: #TimeWarsLaunch, #FightForTime, #TimeIsPower.
        - Realizar sorteos de skins exclusivas para incentivar la participación y aumentar la visibilidad.
    2. **Colaboraciones en Streaming**
        - Organizar una serie de **eventos de lanzamiento con streamers** en Twitch y YouTube.
        - Crear un **torneo inaugural** con premios de skins exclusivas y recompensas estéticas para el primer campeón.
    3. **Estrategia de Influencers**
        - Firmar acuerdos con streamers conocidos para realizar **streams regulares** durante la semana de lanzamiento.
        - Proveerles de códigos para que puedan distribuir skins exclusivas a sus seguidores, incentivando la participación.
    4. **Marketing en Tiendas Digitales**
        - Posicionar el juego en tiendas como **Steam y Epic Games Store** con banners de lanzamiento y promociones en portada.
        - Incluir **reseñas anticipadas** de influencers y jugadores de la beta para agregar credibilidad.
    5. **Campaña de Publicidad en Plataformas Digitales**
        - Implementar anuncios en plataformas como Google Ads y redes sociales, orientados a jugadores de juegos competitivos.
        - Crear **anuncios en video** que muestren las mecánicas de tiempo y el combate, así como las personalizaciones disponibles.
    
    > Fase 3: Post-Lanzamiento
    
    El objetivo de esta fase es mantener la atención de los jugadores, fomentar la comunidad y crear un flujo constante de contenido.
    
    1. **Eventos In-Game Regulares**
        - Introducir **eventos de tiempo limitado** que giren en torno a diferentes temáticas (ej. evento de invasión de jefes, desafíos de oleadas infinitas).
        - Implementar **torneos** con premios estéticos únicos para los ganadores.
        - Crear eventos de con dobles recompensas o desafíos exclusivos.
    2. **Lanzamiento de Skins y Cosméticos por Temporada**
        - Cada temporada del juego debe incluir un set de skins temáticas y efectos visuales que se introduzcan progresivamente.
        - Anunciar las skins con **pequeños videos de presentación** y **streams** que muestren el diseño de cada skin.
    3. **Battle Pass y Contenido Temático**
        - Lanzar un **Battle Pass** para cada temporada con recompensas exclusivas que sigan un tema concreto.
        - Promover el pase en redes sociales y dentro del juego con banners y promociones especiales.
    4. **Estrategia de Retención y Comunidad**
        - Organizar **streams con desarrolladores** para hablar sobre las próximas actualizaciones, mecánicas y responder preguntas de la comunidad.
        - Crear un **canal de feedback** en Discord para recopilar opiniones y sugerencias de la comunidad.
        - Implementar una **tabla de líderes semanal** y destacar a los jugadores más destacados en las redes sociales oficiales.

3. **Publicidad y Promoción**
    1. **Anuncios en Video**:
        - Crear **trailers cinemáticos** y **gameplays explicativos** que se distribuyan en plataformas como YouTube, TikTok y Twitter.
        - Publicidad en redes como **Instagram** y **TikTok** orientada a la audiencia joven.
    2. **Campaña de CPC** (Coste por Clic):
        - Publicar anuncios dirigidos en Google Ads, enfocados en palabras clave como “juego de arena competitivo”, “free to play shooter”, etc.
        - Campañas en plataformas de videojuegos como **Reddit** y **Foros especializados**.
    3. **Marketing de Contenido**:
        - Publicar artículos en blogs y foros sobre el diseño del juego, la mecánica de tiempo y su enfoque en el juego justo.
        - Crear un **diario de desarrolladores** con vídeos de detrás de cámaras.

4. **Presupuesto de Marketing**

    Es importante asignar un presupuesto adecuado para cada parte del plan de marketing. Algunas categorías incluyen:

    - **Publicidad digital**: Anuncios en redes sociales y Google Ads (30% del presupuesto total).
    - **Colaboraciones con influencers**: Costes de patrocinio y productos (20% del presupuesto total).
    - **Creación de contenido**: Producción de trailers, gráficos y material promocional (25% del presupuesto total).
    - **Eventos y premios**: Costes asociados a torneos y eventos promocionales (25% del presupuesto total).

5. **Evaluación y Métricas de Éxito**

    Para medir el éxito del plan de marketing, se deben establecer métricas clave (KPIs) que permitan evaluar el rendimiento:
    
    - **Crecimiento de la comunidad**: Número de seguidores en redes sociales y en el servidor de Discord.
    - **Tasa de retención**: Porcentaje de jugadores que regresan al juego después de la primera semana y el primer mes.
    - **Ingresos**: Ventas generadas a través de microtransacciones y el Battle Pass.
    - **Interacción en eventos**: Número de jugadores participando en torneos y eventos especiales.
    - **Feedback de la comunidad**: Opiniones y sugerencias recopiladas a través de encuestas y foros.

## 4. Narrativa
### Mundo

## 5. Gameplay
### Jugabilidad	
### Sistema HUD
### Personajes jugables	
### Personajes enemigos
### Controles

## 6. Interfaces y pantallas
### Pantalla de inicio	
<div align="center">
   <img src="Assets/Arte/Interfaces/InterfazInicio.png" alt="Interfaz Inicio" width="500" height="250">
</div>

### Tutoriales	

### Diagrama de flujo de interfaces	

### Interfaz de juego	
<div align="center">
   <img src="Assets/Arte/Interfaces/InterfazJuego.png" alt="Interfaz Juego" width="500" height="250">
</div>

### Pantallas de Victoria / Derrota	
<div align="center">
   <img src="Assets/Arte/Interfaces/InterfazVictoria.png" alt="Interfaz Victoria" width="500" height="250">
</div>

<div align="center">
   <img src="Assets/Arte/Interfaces/InterfazDerrota.png" alt="Interfaz Derrota" width="500" height="250">
</div>

### Pantalla de selección de personaje	
<div align="center">
   <img src="Assets/Arte/Interfaces/InterfazSeleccionPersonaje.png" alt="Interfaz Seleccion Personaje" width="500" height="250">
</div>

### Pantalla de personalización	
<div align="center">
   <img src="Assets/Arte/Interfaces/InterfazPersonalizacion.png" alt="Interfaz Personalizacion" width="500" height="250">
</div>

### Pantalla de Battle Pass	
<div align="center">
   <img src="Assets/Arte/Interfaces/InterfazBattlePass.png" alt="Interfaz Battle Pass" width="500" height="250">
</div>

### Pantalla de compra	
<div align="center">
   <img src="Assets/Arte/Interfaces/InterfazCompra.png" alt="Interfaz Compra" width="500" height="250">
</div>

### Pantalla de carga	
<div align="center">
   <img src="Assets/Arte/Interfaces/InterfazCarga.png" alt="Interfaz Carga" width="500" height="250">
</div>

### Pantalla de opciones	
<div align="center">
   <img src="Assets/Arte/Interfaces/InterfazOpciones.png" alt="Interfaz Opciones" width="500" height="250">
</div>

### Pantalla de controles	
<div align="center">
   <img src="Assets/Arte/Interfaces/InterfazControles.png" alt="Interfaz Controles" width="500" height="250">
</div>

### Pantalla de usuario	
<div align="center">
   <img src="Assets/Arte/Interfaces/InterfazUsuario.png" alt="Interfaz Usuario" width="500" height="250">
</div>

### Pantalla de créditos	
<div align="center">
   <img src="Assets/Arte/Interfaces/InterfazCreditos.png" alt="Interfaz Creditos" width="500" height="250">
</div>

### Diagrama de flujo de pantallas

## 7. Diseño de escenario 
### Conceptos de escenarios
### Gray Boxing
### Props
### Spawn de enemigos

## 8. Arte 2D
### Arte conceptual
#### Logos
<u>Logo de la empresa:</u>
Para el logo de la empresa se han desarrollado diferentes ideas sobre cómo podría ser repensado el nombre “NEOCORE”:
<div align="center">
   <img src="Assets/Arte/Logos/LogoVersionRechazada4.png" alt="Logo1" width="350" height="300">
</div>

<div align="center">
   <img src="Assets/Arte/Logos/LogoVersionRechazada3.jpg" alt="Logo2" width="600" height="400">
</div>

<div align="center">
   <img src="Assets/Arte/Logos/LogoVersionRechazada2.jpg" alt="Logo3" width="350" height="325">
</div>

<div align="center">
   <img src="Assets/Arte/Logos/LogoVersionRechazada1.jpg" alt="Logo4" width="350" height="350">
</div>

Finalmente se optó por una representación más simple con la representación de joysticks en las “O”:
<p align="center">
  <img src="Assets/Arte/Logos/NeoCoreVersiónBlanca.jpg" alt="LogoFinal1" width="320" height="250">
  <img src="Assets/Arte/Logos/NeoCoreVersiónAzul.jpg" alt="LogoFinal2" width="320" height="250">
</p>

<u>Logo del juego:</u>

#### Personajes jugables
<u>Niño Agua:</u>
<div align="center">
   <img src="Assets/Arte/Personajes/NiñoAgua.jpg" alt="Niño Agua" width="650" height="375">
</div>

<u>Robot Tostadora:</u>
<div align="center">
   <img src="Assets/Arte/Personajes/RobotTostadora.jpg" alt="Robot Tostadora" width="750" height="250">
</div>

#### Enemigos
<div align="center">
   <img src="Assets/Arte/Enemigos/EnemigoBase.png" alt="Enemigo Base" width="500" height="400">
</div>

### Paleta de colores
### Texturas

## 9. Arte 3D
### Modelado personajes jugables
### Modelado enemigos
### Modelado de escenario

## 10. Animaciones
### Rigging
### Frames de animación

## 11. Arte técnico
### VFX
### Iluminación

## 12. Música y sonudi
### Temas musicales
### Efectos de sonido

## 13. Integrantes del equipo
### Manuel Alejandro Villalba Cruz	
### Alejandro Tobías Márquez	
### Raúl González Suero	
### Andrea Gallardo Lasso	
### Daniel Borrego Cruz	
### Blanca García Vera

