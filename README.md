# **Digivice** - *A segunda tela do seu Digimon World 2003*

![GitHub downloads](https://img.shields.io/github/downloads/jeanheck/digivice-backend/total)

Digimon World 2003 esconde muita coisa atrás de menus e achismos. Digivice é uma segunda tela para esse mundo: enquanto você joga no Duckstation, mostra o estado atual do seu grupo, o mapa, suas digievoluções e o quanto você avançou na história — como uma tela de acompanhamento de Nintendo 3DS teria sido, se a era do PS1 tivesse uma.

- Veja a EXP que cada um dos seus Digimons precisa para o próximo nível
- Descubra os inimigos que você pode encontrar em cada mapa
- Tenha um diário de missões principais e secundárias que acompanha automaticamente seu progresso
- Veja árvores de digievolution e requisitos para cada Digimon do seu grupo
- Seja avisado quando um Leilão estiver acontecendo
- Tenha ferramentas para se locomover no Seabed ou no Deserto Mobius
- E mais — os detalhes que o jogo torna difíceis de ver, sempre ao lado do jogo

É o companheiro que você gostaria de ter tido quando começou a percorrer o Asuka Server.

Você pode ver mais no vídeo abaixo:

[![Digivice 3.0.0 (Último lançamento principal)](https://img.youtube.com/vi/a_r7qz0TodQ/hqdefault.jpg)](https://www.youtube.com/watch?v=a_r7qz0TodQ)

## Estado atual

**No momento, o Digivice suporta totalmente o jogo apenas até o Setor Oeste do Asuka Server** (a terceira parte da história). A missão principal, inimigos do mapa e dados relacionados estão completos apenas neste setor.<br>
A partir desse ponto, muitas funcionalidades continuam funcionando, mas outras estarão ausentes. As próximas áreas serão preenchidas em atualizações futuras.

## Roadmap

Mais Digivice está a caminho. Aqui está uma prévia do que está planejado — detalhes e prazos podem mudar conforme o desenvolvimento continua.

### 3.5 — Batalhas de cartas e o mundo ao redor

O jogo de cartas de Digimon World 2003, finalmente com uma segunda tela para acompanhar.

- **Ferramentas de TCG** — suporte ao jogo de cartas dentro do Digivice
- **Dados de batalha mais ricos** — informações detalhadas sobre lutas Digimon vs Digimon
- **Localizador de treinadores** — encontre treinadores prontos para batalhar com cartas ou com seus Digimons
- E mais — pequenas melhorias de qualidade de vida pelo caminho

### 4.0 — A história alcança Asuka City

Um grande avanço para quem está jogando a missão principal.

- **Missão principal até a A.o.A** — diário e acompanhamento mapeados até a batalha da A.o.A em Asuka City
- **Barras de DVEXP que fazem sentido** — corrigidas para refletir corretamente o Tier de cada digievolution
- E mais — a próxima camada de polimento quando o teto do Setor Sul for removido

## Antes de continuar

**Isso suporta apenas Digimon World 2003. Não funcionará com Digimon World 3.**<br>
Os dados do jogo aqui vêm de fontes públicas na web (veja Créditos dos Dados de Referência abaixo). Você pode reutilizar qualquer um deles nos seus próprios projetos.<br>
Eu não possuo direitos sobre o jogo ou sobre Digimon. Até o nome "Digivice" vem desse universo. **Todos os direitos pertencem aos seus respectivos donos.** Este é um projeto de estudo — aprendizado combinado com um jogo que marcou minha infância.

## Requisitos

**Plataforma:** Windows. Digivice é um `.exe` que você executa no seu PC.<br>
**Emulador:** **Deve ser o Duckstation.** Suporte para outros emuladores pode chegar no futuro.

## Como usar

1. Configure o Duckstation para rodar um jogo.
2. Vá em **Settings → Advanced → Interface Settings** e ative **Show Debug Menu**.
3. Um menu **Debugging** aparecerá. Abra-o.
4. Ative **Export Memory Shared**.
5. Baixe o último Digivice na aba **Releases** e extraia o ZIP em uma pasta própria.
6. Inicie Digimon World 2003 no Duckstation.
7. Jogue até seus Digimons estarem com você no mapa (em um novo jogo, depois de ser enviado ao Laboratório Digimon).
8. Com o jogo rodando, clique duas vezes em `digivice.exe`.
9. Permita que o Windows execute se solicitado.
10. Se tudo estiver configurado corretamente, a janela do Digivice deve abrir e funcionar.

## Streaming e gravação (OBS)

O Digivice funciona bem como fonte no OBS, mas algumas peculiaridades do Windows confundem a maioria das pessoas na primeira vez.

1. **Abra o Digivice antes de adicionar a fonte.** O OBS só lista janelas que estão abertas e *não minimizadas*. Se o Digivice estiver minimizado quando você criar a fonte de **Window Capture**, ele simplesmente não aparecerá no dropdown — sem erro, só está ausente. Restaure a janela e reabra o dropdown.
2. **Não minimize durante a transmissão.** O Windows para de desenhar janelas minimizadas, então a captura congela no último frame ou fica preta. Mova o Digivice para um segundo monitor ou deixe-o atrás de outra janela.
3. **Se a captura estiver preta, mude o método de captura.** Nas propriedades da fonte, defina **Capture Method** como **Windows 10 (1903 and up)**. A interface do Digivice é acelerada por GPU e o método mais antigo BitBlt não consegue lê-la.
4. **Defina Window Match Priority** como **Match title, otherwise find window of same executable**, para que a fonte reconecte automaticamente da próxima vez que você abrir o Digivice.

**Game Capture não funciona** com o Digivice, pois faz hook direto no Direct3D/OpenGL. Use **Window Capture** ou **Display Capture**.

## Créditos dos dados de referência

Este projeto utiliza dados públicos do jogo de diversas fontes da web. As originais estão listadas abaixo caso queira explorá-las.

Nada disso teria sido possível sem as pessoas que descobriram e compartilharam essas informações. Obrigado a todos.

- **Níveis de experiência** *por Mehdi*: [GameFAQs](https://gamefaqs.gamespot.com/boards/562323-digimon-world-3/64473556)
- **Digimon World 3 – Digivolution Guide** *por dudeice997*: [GameFAQs](https://gamefaqs.gamespot.com/ps/562323-digimon-world-3/faqs/72629)
- **Digimon World 3/Enemy**: [DigimonWiki](https://digimon.fandom.com/wiki/Digimon_World_3/Enemy)
- **Digimon World 3 – Bestiary** *por TheFulgorah*: [GameFAQs](https://gamefaqs.gamespot.com/ps/562323-digimon-world-3/faqs/66315)
- **dmw3-tools** *por markisha64* [GitHub](https://github.com/markisha64/dmw3-tools)
- **Attributes**: [StratsWiki](https://stratswiki.com/digimon-world-3/game-mechanics/attributes/)
- **All Weapons & Shields**: [StratsWiki](https://stratswiki.com/digimon-world-3/items/weapons-shields/)
- **Body Gear**: [StratsWiki](https://stratswiki.com/digimon-world-3/items/body-gear/)
- **Head Gear**: [StratsWiki](https://stratswiki.com/digimon-world-3/items/head-gear/)
- **Accessories**: [StratsWiki](https://stratswiki.com/digimon-world-3/items/accessories/)
- **GameShark Codes for Digimon World 2003 (PAL VERSION)** *por Iamhim*: [GameFAQs](https://gamefaqs.gamespot.com/boards/562323-digimon-world-3/80568401)
- **Digimon World 2003 PAL VERSION GAMESHARK CODES (CONVERTED)** *por splakappa*: [GameFAQs](https://gamefaqs.gamespot.com/boards/562323-digimon-world-3/62602581)
- **ENTENDA AS FRAQUEZAS E TOLERÂNCIAS NO DIGIMON WORLD 3** *por Battle Dice*: [YouTube](https://www.youtube.com/watch?v=6UbWt7AyHMI)
- **Digimon World 3 – Item List** *por Med_Jai*: [GameFAQs](https://gamefaqs.gamespot.com/ps/562323-digimon-world-3/faqs/24593)
- **Digimon World 3 – FAQ (European)** *por Mykas0*: [GameFAQs](https://gamefaqs.gamespot.com/ps/562323-digimon-world-3/faqs/21889)
- **Digimon World 3 – Guides and FAQs** *por nick1n*: [GameFAQs](https://gamefaqs.gamespot.com/ps/562323-digimon-world-3/map/16028-underground-seabed-map)
- **Digimon World 3 – Detonado Completo (Guia Passo a Passo)** *por Markus Norat*: [revolutionarena](https://revolutionarena.com/pt-br/digimon-world-3-detonado-completo-guia-passo-a-passo/)
- **Digimon World 3 (PS1 Gameplay Português PT-BR)** *por DashGames*: [YouTube](https://www.youtube.com/watch?v=rW8wxZceOjI&list=PLZDYs951OahD_TT4QUByvOEBTRJE-amAi)
- **DRI locations** *por rentz14*: [NeoSeeker](https://www.neoseeker.com/forums/3546/t1102277-dri-locations/)
- **Digimon World 3 – Guides and FAQs** *por HRahman*: [GameFAQs](https://gamefaqs.gamespot.com/ps/562323-digimon-world-3/map/565-mobius-desert-map)
- **Digivolution Experience Points (DVEXP)** *por Rob01m*: [GameFAQs](https://gamefaqs.gamespot.com/boards/562323-digimon-world-3/47657837)

## Créditos das imagens dos Digimons

Abaixo estão os links de onde as imagens dos Digimons foram retiradas, bem como os nomes dos usuários que postaram as respectivas imagens (algumas são Artes Oficiais da Bandai).

### Inimigos

- **Airdramon(Gold)** *por ElissabethHaser* [Deviant Art](https://www.deviantart.com/elissabethhaser/art/Airdramon-Gold-World-3-1001557156)
- **Airdramon** *por ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Airdramon/Galeria?file=Airdramon_b.jpg)
- **Andromon** *por ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Andromon/Galeria?file=Andromon_b.jpg)
- **Apemon** *por Yuetmoon* [Digimon Wiki](https://digimon.fandom.com/wiki/Apemon?file=Apemon_b.jpg)
- **Bakemon** *por ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Bakemon/Galeria?file=Bakemon_b.jpg)
- **Baromon** *por ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Baromon?file=Baromon_b.jpg)
- **Betamon** *por ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Betamon/Galeria?file=Betamon_b.jpg)
- **Bulbmon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Bulbmon?file=Bulbmon_b.jpg)
- **Clockmon** *por ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Clockmon/Galeria?file=Clockmon_b.jpg)
- **Coelamon** *por ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Coelamon/Galeria?file=Coelamon_b.jpg)
- **Crabmon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Crabmon?file=Crabmon_b.jpg)
- **Cyclonemon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Cyclonemon?file=Cyclonemon_b.jpg)
- **Datamon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Datamon?file=Datamon_b.jpg)
- **DemiDevimon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/DemiDevimon?file=DemiDevimon_b.jpg)
- **Divermon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Divermon?file=Divermon_b.jpg)
- **Dokugumon(Brown)** *por eu (usando a imagem do Dokugumon como base, apenas mudando as cores para tons marrons)*
- **Dokugumon** *por Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Dokugumon?file=Dokugumon_b.jpg)
- **Dolphmon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Dolphmon?file=Dolphmon_b.jpg)
- **Etemon** *por Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Etemon?file=Etemon_b.jpg)
- **Flymon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Flymon?file=Flymon_b.jpg)
- **Gekomon** *por ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Gekomon/Galeria?file=Gekomon_b.jpg)
- **Gesomon** *por ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Gesomon/Galeria?file=Gesomon_b.jpg)
- **Gizamon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Gizamon?file=Gizamon_b.jpg)
- **Goburimon(Red)** *por DoruFlaymDraGon* [Deviant Art](https://www.deviantart.com/doruflaymdragon/art/Goburimon-Red-red-hair-913908979)
- **Goburimon** *por Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Goburimon/Galeria?file=Goburimon_b.jpg)
- **Hagurumon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Hagurumon?file=Hagurumon_b.jpg)
- **HiAndromon** *por ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Hi-Andromon/Galeria?file=Hi-Andromon_b.jpg)
- **Kiwimon** *por ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Kiwimon/Galeria?file=Kiwimon_b.jpg)
- **Kokatorimon(Brown)** *por Digital Monsters Almanac* [Digital Monsters Almanac](http://dma.wtw-x.net/dexkokatori.shtml)
- **Kokatorimon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Kokatorimon?file=Kokatorimon_b.jpg)
- **Kunemon** *por ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Kunemon/Galeria?file=Kunemon_b.jpg)
- **Kuwagamon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Kuwagamon?file=Kuwagamon_b.jpg)
- **Maildramon** *por Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Maildramon?file=Maildramon_b.jpg)
- **Mamemon** *por ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Mamemon/Galeria?file=Mamemon_b.jpg)
- **Mammothmon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Mammothmon?file=Mammothmon_b.jpg)
- **MasterTyranomon** *por Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/MasterTyranomon?file=MasterTyranomon_b.jpg)
- **Minotarumon** *por ShikaSS* [Digimon Wiki](https://digimon.fandom.com/pt/wiki/Lista_de_Esp%C3%A9cies?file=Minotaurmon_b.jpg)
- **Musyamon** *por ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Musyamon/Galeria?file=Musyamon_b.jpg)
- **Numemon(Blue)** *por Digital Monsters Almanac* [Digital Monsters Almanac](http://dma.wtw-x.net/dexnume.shtml)
- **Numemon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Numemon?file=Numemon_b.jpg)
- **Ogremon** *por Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Orgemon/Galeria?file=Orgemon_b.jpg)
- **Pharaohmon** *por Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Pharaohmon?file=Pharaohmon_b.jpg)
- **Raremon** *por Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Raremon?file=Raremon_b.jpg)
- **RedVegiemon** *por Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/RedVegimon/Galeria?file=RedVegimon_b.jpg)
- **Seadramon** *por Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Seadramon?file=Seadramon_b.jpg)
- **Shellmon** *por Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Shellmon/Galeria?file=Shellmon_b.jpg)
- **ShogunGekomon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/ShogunGekomon?file=ShogunGekomon_b.jpg)
- **Sukamon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Sukamon?file=Sukamon_b.jpg)
- **Tapirmon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Tapirmon?file=Tapirmon_b.jpg)
- **Thundermon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Thundermon?file=Thundermon_b.jpg)
- **Triceramon** *por Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Triceramon?file=Triceramon_b.jpg)
- **Tuskmon** *por Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Tuskmon?file=Tuskmon_b.jpg)
- **Tyrannomon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Tyrannomon?file=Tyrannomon_b.jpg)
- **Vademon** *por Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Vademon/Galeria?file=Vademon_b.jpg)
- **Vegiemon** *por Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Vegimon/Galeria?file=Vegimon_b.jpg)
- **WaruMonzaemon** *por ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/WaruMonzaemon?file=WaruMonzaemon_b.jpg)
- **Woodmon(Green)** *por Digital Monsters Almanac* [Digital Monsters Almanac](http://dma.wtw-x.net/dexwood.shtml)
- **Woodmon** *por Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Woodmon?file=Woodmon_b.jpg)
- **Yanmamon(Green)** *por eu (usando a imagem do Yanmamon como base, apenas mudando as cores para tons verdes)*
- **Yanmamon** *por ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Yanmamon/Galeria?file=Yanmamon_b.jpg)
- **Zanbamon** *por Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Zanbamon?file=Zanbamon_b.jpg)

### Digievoluttions

- **Angemon** *por G-SANtos* [Digimon Wiki](https://digimon.fandom.com/wiki/Angemon?file=Angemon_b.jpg)
- **Angewomon** *por Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Angewomon/Galeria?file=Angewomon_b.jpg)
- **Armormon** *por Chimera-gui* [Digimon Wiki](https://digimon.fandom.com/wiki/Armormon?file=Armormon_b.jpg)
- **Beelzemon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Beelzemon?file=Beelzemon+b.jpg)
- **BK WarGreymon** *por Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/BlackWarGreymon?file=BlackWarGreymon_b.jpg)
- **Cannondramon** *por ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Cannondramon/Galeria?file=Cannondramon_b.jpg)
- **Devimon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Devimon?file=Devimon+b.jpg)
- **Diaboromon** *por ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Diablomon?file=Diablomon_b.jpg)
- **Digitamamon** *por Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Digitamamon?file=Digitamamon_b.jpg)
- **Dinohumon** *por ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Dinohumon?file=Dinohumon_b.jpg)
- **ExVeemon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/ExVeemon?file=ExVeemon_b.jpg)
- **Gallantmon** *por Lanate* [Digimon Wiki](https://digimon.fandom.com/wiki/Gallantmon?file=Gallantmon+b.jpg)
- **GranKuwagamon** *por ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/GranKuwagamon?file=GranKuwagamon_b.jpg)
- **GrapLeomon** *por Alfius* [Digimon Wiki](https://digimon.fandom.com/wiki/GrapLeomon?file=GrapLeomon_b.jpg)
- **Greymon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Greymon?file=Greymon_b.jpg)
- **Grizzlymon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Grizzlymon?file=Grizzlymon_b.jpg)
- **Growlmon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Growlmon?file=Growlmon_b.jpg)
- **Guardiangemon** *por Magnaillusion CM* [NeoSeeker](https://digimon.neoseeker.com/wiki/File:Slashangemon.jpg)
- **Hookmon** *por ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Hookmon?file=Hookmon_b.jpg)
- **Imperialdramon** *por Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Imperialdramon:_Dragon_Mode?file=Imperialdramon_Dragon_Mode_b.jpg)
- **Imperialdramon-F** *por Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Imperialdramon:_Fighter_Mode?file=Imperialdramon_Fighter_Mode_b.jpg)
- **Imperialdramon-P** *por Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Imperialdramon:_Paladin_Mode?file=Imperialdramon_Paladin_Mode_b.jpg)
- **Kabuterimon** *por Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Kabuterimon?file=Kabuterimon_b.jpg)
- **Kyubimon** *por ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Kyubimon?file=Kyubimon_b.jpg)
- **Kyukimon** *por ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Kyukimon?file=Kyukimon_b.jpg)
- **MaloMyotismon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/MaloMyotismon?file=MaloMyotismon_b.jpg)
- **Marsmon** *por Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Marsmon?file=Marsmon_b.jpg)
- **MetalGarurumon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/MetalGarurumon?file=MetalGarurumon_b.jpg)
- **MetalGreymon** *por Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/MetalGreymon_(Vaccine)?file=MetalGreymon_%28Vaccine%29_b.jpg)
- **MetalMamemon** *por Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/MetalMamemon?file=MetalMamemon_b.jpg)
- **Myotismon** *por G-SANtos* [Digimon Wiki](https://digimon.fandom.com/wiki/Myotismon?file=Myotismon_b.jpg)
- **Omnimon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Omnimon?file=Omnimon_b.jpg)
- **Paildramon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Paildramon?file=Paildramon_b.jpg)
- **Phoenixmon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Phoenixmon?file=Phoenixmon_b.jpg)
- **Rosemon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Rosemon?file=Rosemon_b.jpg)
- **Sakuyamon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Sakuyamon?file=Sakuyamon_b.jpg)
- **Seraphimon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Seraphimon?file=Seraphimon_b.jpg)
- **SkullGreymon** *por ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/SkullGreymon?file=SkullGreymon_b.jpg)
- **Stingmon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/Stingmon?file=Stingmon_b.jpg)
- **Taomon** *por Kamirisu JxS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/Taomon?file=Taomon_b.jpg)
- **WarGreymon** *por ShikaSS* [Digimon Wiki](https://digitalmonster.fandom.com/pt/wiki/WarGreymon?file=WarGreymon_b.jpg)
- **WarGrowlmon** *por KrytenKoro* [Digimon Wiki](https://digimon.fandom.com/wiki/WarGrowlmon?file=WarGrowlmon_b.jpg)
