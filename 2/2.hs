import Text.Printf (printf)
import Text.Read.Lex (Number)
deltaXFromTwoYears :: Float -> Float -> Float
deltaXFromTwoYears current prev = current - prev


deltaXFromAlphaAndBeta :: Float -> Float -> Float -> Float
deltaXFromAlphaAndBeta cur a b = (a * cur) + (b * cur)

alpha :: Float -> Float -> Float
alpha all born  = born / all

beta :: Float -> Float -> Float
beta all dead = dead / all


gammaFromAlphaAndBeta :: Float -> Float -> Float
gammaFromAlphaAndBeta a b = 1 + a - b


newXFromXAndGamma :: Float -> Float -> Float
newXFromXAndGamma x g = g * x

logisticNewX :: Float  -> Float -> Float -> Float
logisticNewX current gamma max = current + gamma * (1 - current/max) * current


logisticModel :: Float  -> Float -> Float -> [Float]
logisticModel current gamma max = current : logisticModel new gamma max where new = logisticNewX current gamma max


model :: Float -> Float -> [Float]
model current gamma = current : model new gamma where new = newXFromXAndGamma current gamma





countDoublings :: (Ord a, Num a) => [a] -> [Int]
countDoublings (x:xs)
    | null remaining = []
    | otherwise = length took : countDoublings remaining
    where (took, remaining) = span (< x * 2) xs


countDoublings [] = []






printLineByLine :: Show a => [a] -> IO ()

printLineByLine (x:xs)  = do
    print x
    printLineByLine xs


printLineByLineWithNumber :: Show a => [a] -> Int -> IO ()

printLineByLineWithNumber (x:xs) lineNum  = do

    printf "%d) %s\n" lineNum $ show x
    printLineByLineWithNumber xs $ lineNum + 1

printLineByLineWithNumber [] _ = return ()







main :: IO ()
main = do


    let days = 100

    let initalPopulation = 100000

    let gammaValue = 1.02

    let modelValues = take days $ model initalPopulation gammaValue



    print 1
    printLineByLineWithNumber modelValues 1


    printf "A) amounts of days between population doublings %s (%d days passed in total)\n" (show $ countDoublings  modelValues) days


    let dayOfChange = 10
    let change = 0.8
    let changed = modelValues !!  (dayOfChange - 1) * change
    let changeIndex = dayOfChange - 1

    printf "B) the population was restored on day %d\n" $ length $ takeWhile (< modelValues !! changeIndex) $ take  changeIndex modelValues ++ model changed gammaValue



    let maxPopulation = 100000000

    let logisticModelValues = take days $ logisticModel 30 gammaValue maxPopulation

    printf "C)\n"

    printLineByLineWithNumber logisticModelValues 1

    return ()





